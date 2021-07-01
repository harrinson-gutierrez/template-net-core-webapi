using Application.DTOs.Authentication;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Resources;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Settings;
using Google.Apis.Auth;
using Infrastructure.Adapter.Email.Interfaces;
using Infrastructure.Adapter.Email.Models;
using Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> UserManagerService;
        private readonly SignInManager<AppUser> SignInManagerService;
        private readonly IPasswordHasher<AppUser> PasswordHasher;
        private readonly IAppResource AppResource;
        private readonly IOptions<CustomJwtOptions> CustomJwtOptions;
        private readonly TokenValidationParameters TokenValidationParameters;
        private readonly ILogger Logger;
        private readonly IRefreshTokenRepository RefreshTokenRepository;
        private readonly IEmailService EmailService;
        private readonly IOptions<AuthenticationOptions> AuthenticationConfiguration;
        private readonly IUserRepository UserRepository;
        private readonly IAuthFacebookService FacebookAuthService;

        public AuthenticationService(UserManager<AppUser> userManager,
                                     SignInManager<AppUser> signInManager,
                                     IPasswordHasher<AppUser> passwordHasher,
                                     IAppResource appResource,
                                     IOptions<CustomJwtOptions> customJwtOptions,
                                     TokenValidationParameters tokenValidationParameters,
                                     ILoggerFactory logFactory,
                                     IRefreshTokenRepository refreshTokenRepository,
                                     IEmailService emailService,
                                     IOptions<AuthenticationOptions> authenticationConfiguration,
                                     IUserRepository userRepository,
                                     IAuthFacebookService facebookAuthService)
        {
            UserManagerService = userManager;
            SignInManagerService = signInManager;
            PasswordHasher = passwordHasher;
            AppResource = appResource;
            CustomJwtOptions = customJwtOptions;
            TokenValidationParameters = tokenValidationParameters;
            Logger = logFactory.CreateLogger<AuthenticationService>();
            RefreshTokenRepository = refreshTokenRepository;
            EmailService = emailService;
            AuthenticationConfiguration = authenticationConfiguration;
            UserRepository = userRepository;
            FacebookAuthService = facebookAuthService;
        }

        public async Task<AuthenticationResponse> SignInUserAsync(AuthenticationLoginRequest authenticationLogin)
        {
            if (authenticationLogin.GrantType.Equals(GrantType.Password))
            {
                return await GenerateAccessToken(authenticationLogin.Email, authenticationLogin.Password);
            }
            else if (authenticationLogin.GrantType.Equals(GrantType.RefreshToken))
            {
                return await GenerateRefreshToken(authenticationLogin.AccessToken, authenticationLogin.RefreshToken);
            }
            else
                throw new ApiException(AppResource["ExceptionInvalidRequestGrantTypeNotFound"]);
        }

        public async Task<UserRegistrationResponse> SignUpUserAsync(UserRegistrationRequest userRegistrationRequest)
        {
            AppUser newUser = new AppUser()
            {
                email = userRegistrationRequest.Email,
                username = userRegistrationRequest.Email,
                email_confirmed = false
            };

            newUser.password_hash = PasswordHasher.HashPassword(newUser, userRegistrationRequest.Password);

            IdentityResult createdUser = await UserManagerService.CreateAsync(newUser);

            if (createdUser.Succeeded)
            {
                await UserManagerService.AddToRolesAsync(newUser, new List<string> { "USER" });

                string token = await UserManagerService.GenerateEmailConfirmationTokenAsync(newUser);

                string route = string.Format(AuthenticationConfiguration.Value.FrontEndUrl + AuthenticationConfiguration.Value.ActivateAccountRoute, newUser.email, WebUtility.UrlEncode(token));

                await EmailService.SendEmailWithTemplate(new EmailTemplateRequest()
                {
                    Subject = AppResource["EmailConfirmedAccountSubject"],
                    Receivers = new List<string>() { newUser.email},
                    Data = new { token = route , newUser.username, frontUrl = AuthenticationConfiguration.Value.FrontEndUrl + AuthenticationConfiguration.Value.Logo },
                    Template = AppResource["EmailConfirmedAccountTemplate"]
                });
            }   

            return new UserRegistrationResponse()
            {
                Success = createdUser.Succeeded,
                Errors = createdUser.Errors.Select(x => x.Description).ToList()
            };
        }

        public async Task<EmailResponse> RecoveryPasswordAsync(RecoveryPasswordRequest recoveryPasswordRequest) 
        {
            AppUser user = await UserManagerService.FindByEmailAsync(recoveryPasswordRequest.Email);

            if (user == null)
                throw new ApiException(string.Format(AppResource["ExceptionUserNotFound"], recoveryPasswordRequest.Email));

            if (!user.email_confirmed)
                throw new ApiException(AppResource["ExceptionInvalidRequestEmailNotConfirmed"]);

            string token = await UserManagerService.GeneratePasswordResetTokenAsync(user);

            return await EmailService.SendEmailWithTemplate(new EmailTemplateRequest()
            {
                Subject = AppResource["EmailRecoveryPasswordSubject"],
                Receivers = new List<string>() { user.email},
                Data = new {user.username, token},
                Template = AppResource["EmailRecoveryPasswordTemplate"]
            });
        }

        public async Task<IdentityResult> RecoveryPasswordConfirmedAsync(RecoveryPasswordConfirmRequest recoveryPasswordConfirmRequest)
        {
            AppUser user = await UserManagerService.FindByEmailAsync(recoveryPasswordConfirmRequest.Email);

            if (user == null)
                throw new ApiException(string.Format(AppResource["ExceptionUserNotFound"], recoveryPasswordConfirmRequest.Email));

            IdentityResult result = await UserManagerService
                                    .ResetPasswordAsync(user, recoveryPasswordConfirmRequest.Token, recoveryPasswordConfirmRequest.Password);

            if (result.Succeeded)
            {
                await EmailService.SendEmailWithTemplate(new EmailTemplateRequest()
                {
                    Subject = AppResource["EmailRecoveryPasswordConfirmSubject"],
                    Receivers = new List<string>() { user.email },
                    Data = new { user.username },
                    Template = AppResource["EmailRecoveryPasswordConfirmTemplate"]
                });
            }

            return result;
        }

        public async Task<EmailResponse> ReSendEmailConfirmationAsync(ReSendEmailConfirmationRequest reSendEmailConfirmationRequest)
        {
            AppUser appUser = await UserManagerService.FindByEmailAsync(reSendEmailConfirmationRequest.Email);

            if (appUser == null)
                throw new ApiException(string.Format(AppResource["ExceptionUserNotFound"], reSendEmailConfirmationRequest.Email));

            if (appUser.email_confirmed)
                throw new ApiException(string.Format(AppResource["ExceptionInvalidRequestUserHasConfirm"], reSendEmailConfirmationRequest.Email));

            string token = await UserManagerService.GenerateEmailConfirmationTokenAsync(appUser);

            string route = string.Format(AuthenticationConfiguration.Value.FrontEndUrl + AuthenticationConfiguration.Value.ActivateAccountRoute, appUser.email, WebUtility.UrlEncode(token));

            return await EmailService.SendEmailWithTemplate(new EmailTemplateRequest()
            {
                Subject = AppResource["EmailConfirmedAccountSubject"],
                Receivers = new List<string>() { appUser.email },
                Data = new { token = route, appUser.username, frontUrl = AuthenticationConfiguration.Value.FrontEndUrl + AuthenticationConfiguration.Value.Logo},
                Template = AppResource["EmailConfirmedAccountTemplate"]
            });
        }

        public async Task<IdentityResult> ConfirmEmailConfirmationAsync(ConfirmEmailConfirmationRequest confirmEmailConfirmationRequest)
        {
            AppUser user = await UserManagerService.FindByEmailAsync(confirmEmailConfirmationRequest.Email);

            if (user == null)
                throw new ApiException(string.Format(AppResource["ExceptionUserNotFound"], confirmEmailConfirmationRequest.Email));

            return await UserManagerService
                                    .ConfirmEmailAsync(user, confirmEmailConfirmationRequest.Token);
        }

        private async Task<AuthenticationResponse> GenerateAccessToken(string email, string password)
        {
            AppUser user = await UserManagerService.FindByEmailAsync(email);

            if (user == null)
                throw new ApiException(AppResource["ExceptionUnauthorizedUser"]);

            if(string.IsNullOrEmpty(user.password_hash))
            {
                var userLogins = await UserRepository.GetAppUserLoginsByUser(user.user_id);
                var textForProviders = string.Join(",", userLogins.Select(x => x.login_provider));
                throw new ApiException(AppResource["UserHasLoginWithProvider", new object[] { textForProviders }]);
            }

            SignInResult signInResult = await SignInManagerService.CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
                throw new ApiException(AppResource["ExceptionUnauthorizedUser"]);

            return await GenerateAuthenticationForUser(user);
        }

        private async Task<AuthenticationResponse> GenerateRefreshToken(string accessToken, string refreshToken)
        {
            ClaimsPrincipal claimsPrincipal = GetPrincipalClaimsFromToken(accessToken);

            if (claimsPrincipal == null)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestInvalidToken"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            long expiryDate = long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            DateTime expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDate);

            if (expiryDateUtc > DateTime.UtcNow)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestHasntExpired"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            string jti = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            RefreshToken storedRefreshToken = await RefreshTokenRepository.GetByIdAsync(Guid.Parse(refreshToken));

            if (storedRefreshToken == null)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestRefreshTokenNotExists"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            if (DateTime.UtcNow > storedRefreshToken.expired_date)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestRefreshTokenExpired"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            if (storedRefreshToken.invalidated)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestRefreshTokenInvalidated"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            if (storedRefreshToken.used)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestRefreshTokenHasUsed"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            if (storedRefreshToken.jwt_id != jti)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestRefreshTokenNotMatch"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            storedRefreshToken.used = true;
            await RefreshTokenRepository.UpdateAsync(storedRefreshToken);

            AppUser appUser = await UserManagerService.FindByIdAsync(claimsPrincipal.Claims.Single(x => x.Type == "id").Value);

            return await GenerateAuthenticationForUser(appUser);
        }

        private async Task<AuthenticationResponse> GenerateAuthenticationForUser(AppUser appUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CustomJwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                    CustomJwtOptions.Value.Issuer,
                    CustomJwtOptions.Value.Audience,
                    await CreateClaimsAsync(appUser),
                    expires: DateTime.UtcNow.AddMinutes(CustomJwtOptions.Value.ExpiredTime),
                    signingCredentials: creds
                );

            RefreshToken refreshToken = new RefreshToken()
            {
                token = Guid.NewGuid(),
                jwt_id = token.Id,
                user_id = appUser.user_id,
                creation_date = DateTime.UtcNow,
                expired_date = DateTime.UtcNow.AddDays(CustomJwtOptions.Value.RefreshTokenExpiredDaysTime)
            };

            await RefreshTokenRepository.CreateAsync(refreshToken);

            return new AuthenticationResponse()
            {
                Type = "Bearer",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.token.ToString(),
                Expiration = token.ValidTo
            };
        }

        private async Task<Claim[]> CreateClaimsAsync(AppUser appUser)
        {
            return new[]
            {
                    new Claim("id", appUser.user_id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, appUser.username),
                    new Claim(JwtRegisteredClaimNames.Email, appUser.email),
                    new Claim("role", string.Join(",", await UserManagerService.GetRolesAsync(appUser)))
                };
        }

        private ClaimsPrincipal GetPrincipalClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out var securityToken);

                if (!IsJwtWithSecurityAlgoritm(securityToken))
                    return null;

                return principal;
            }
            catch(Exception ex)
            {
                Logger.LogError("GetPrincipalClaimsFromToken JWT failed Validation Token {0}", ex.Message);
                return null;
            }
        }

        private bool IsJwtWithSecurityAlgoritm(SecurityToken securityToken)
        {
            return (securityToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                                        StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<IdentityResult> LogoutAsync(LogoutRequest logoutRequest)
        {
            await SignInManagerService.SignOutAsync();

            ClaimsPrincipal claimsPrincipal = GetPrincipalClaimsFromToken(logoutRequest.Token);

            if (claimsPrincipal == null)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestInvalidToken"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            Guid jwtId = Guid.Parse(claimsPrincipal.Claims.Single(x => x.Type == "jti").Value);

            RefreshToken refreshToken = await RefreshTokenRepository.GetByJwtId(jwtId);

            if (refreshToken == null)
            {
                Logger.LogError(AppResource["ExceptionInvalidRequestInvalidToken"]);
                throw new ApiException(AppResource["ExceptionInvalidRequestInvalidToken"]);
            }

            refreshToken.invalidated = true;
            await RefreshTokenRepository.UpdateAsync(refreshToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<AuthenticationResponse> SignInWithGoogleAsync(AuthenticationLoginGoogleRequest authenticationLoginGoogleRequest)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(authenticationLoginGoogleRequest.TokenId, new GoogleJsonWebSignature.ValidationSettings());

                AppUser user = await UserManagerService.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new AppUser()
                    {
                        email = payload.Email,
                        username = payload.Email,
                        email_confirmed = true
                    };

                    IdentityResult createdUser = await UserManagerService.CreateAsync(user);

                    if (createdUser.Succeeded)
                    {
                        await UserManagerService.AddToRolesAsync(user, new List<string> { "USER" });

                        await UserRepository.CreateAppUserLogin(new AppUserLogin()
                        {
                            login_provider = "Google",
                            provider_key = payload.Subject,
                            user_id = user.user_id,
                            provider_displayname = payload.Name
                        });
                    }
                    else
                    {
                        throw new ApiException("Cannot create account with google");
                    }
                }

                var appUserLogin = UserRepository.GetAppUserLoginByUserAndProvider("Google", payload.Subject);

                if(appUserLogin == null)
                {
                    await UserRepository.CreateAppUserLogin(new AppUserLogin()
                    {
                        login_provider = "Google",
                        provider_key = payload.Subject,
                        user_id = user.user_id,
                        provider_displayname = payload.Name
                    });
                }

                return await GenerateAuthenticationForUser(user);
            }
            catch(InvalidJwtException ex)
            {
                Logger.LogError(ex, "Validate Google Token Failure");
                throw new ApiException("Connection with google not complete");
            }
        }

        public async Task<AuthenticationResponse> SignInWithFacebookAsync(AuthenticationLoginFacebookRequest authenticationLoginFacebookRequest)
        {
            try
            {
                var payloadValidate = await FacebookAuthService.ValidateTokenAsync(authenticationLoginFacebookRequest.AccessToken);
                var payloadUser = await FacebookAuthService.GetUserInfoAsync(authenticationLoginFacebookRequest.AccessToken);

                AppUser user = await UserManagerService.FindByEmailAsync(payloadUser.Email);

                if (user == null)
                {
                    user = new AppUser()
                    {
                        email = payloadUser.Email,
                        username = payloadUser.Email,
                        email_confirmed = true
                    };

                    IdentityResult createdUser = await UserManagerService.CreateAsync(user);

                    if (createdUser.Succeeded)
                    {
                        await UserManagerService.AddToRolesAsync(user, new List<string> { "USER" });

                        await UserRepository.CreateAppUserLogin(new AppUserLogin()
                        {
                            login_provider = "Facebook",
                            provider_key = payloadValidate.Data.UserId,
                            user_id = user.user_id,
                            provider_displayname = payloadUser.FirstName + " " + payloadUser.LastName
                        });
                    }
                    else
                    {
                        throw new ApiException("Cannot create account with Facebook");
                    }
                }

                var appUserLogin = await UserRepository.GetAppUserLoginByUserAndProvider("Facebook", payloadValidate.Data.UserId);

                if (appUserLogin == null)
                {
                    await UserRepository.CreateAppUserLogin(new AppUserLogin()
                    {
                        login_provider = "Facebook",
                        provider_key = payloadValidate.Data.UserId,
                        user_id = user.user_id,
                        provider_displayname = payloadUser.FirstName + " " + payloadUser.LastName
                    });
                }

                return await GenerateAuthenticationForUser(user);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Validate Facebook Token Failure");
                throw new ApiException("Connection with google not complete");
            }
        }
    }
}
