using Application.DTOs.Authentication;
using Infrastructure.Adapter.Email.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> SignInUserAsync(AuthenticationLoginRequest authenticationLogin);

        Task<UserRegistrationResponse> SignUpUserAsync(UserRegistrationRequest userRegistrationRequest);

        Task<EmailResponse> RecoveryPasswordAsync(RecoveryPasswordRequest recoveryPasswordRequest);

        Task<IdentityResult> RecoveryPasswordConfirmedAsync(RecoveryPasswordConfirmRequest recoveryPasswordConfirmRequest);

        Task<EmailResponse> ReSendEmailConfirmationAsync(ReSendEmailConfirmationRequest reSendEmailConfirmationRequest);

        Task<IdentityResult> ConfirmEmailConfirmationAsync(ConfirmEmailConfirmationRequest confirmEmailConfirmationRequest);

        Task<IdentityResult> LogoutAsync(LogoutRequest logoutRequest);

        Task<AuthenticationResponse> SignInWithGoogleAsync(AuthenticationLoginGoogleRequest authenticationLoginGoogleRequest);
        Task<AuthenticationResponse> SignInWithFacebookAsync(AuthenticationLoginFacebookRequest authenticationLoginFacebookRequest);
    }
}
