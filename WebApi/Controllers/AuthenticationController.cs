using Application.Binders;
using Application.DTOs.Authentication;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticationLoginRequest authenticationLogin)
        {
            return Ok(await AuthenticationService.SignInUserAsync(authenticationLogin));
        }

        [Route("Login/Google")]
        [HttpPost]
        public async Task<IActionResult> LoginWithGoogle([FromBody] AuthenticationLoginGoogleRequest authenticationLoginGoogleRequest)
        {
            return Ok(await AuthenticationService.SignInWithGoogleAsync(authenticationLoginGoogleRequest));
        }

        [Route("Login/Facebook")]
        [HttpPost]
        public async Task<IActionResult> LoginWithFacebook([FromBody] AuthenticationLoginFacebookRequest authenticationLoginFacebookRequest)
        {
            return Ok(await AuthenticationService.SignInWithFacebookAsync(authenticationLoginFacebookRequest));
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            UserRegistrationResponse response = await AuthenticationService.SignUpUserAsync(userRegistrationRequest);
            if (response.Success)
                return NoContent();
            else
                return BadRequest(new { message = string.Join('\n', response.Errors) });
        }

        [Route("Recovery/Password")]    
        [HttpPost]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordRequest recoveryPasswordRequest)
        {
            if ((await AuthenticationService.RecoveryPasswordAsync(recoveryPasswordRequest)).Success)
                return NoContent();
            else
                return BadRequest();
        }


        [Route("Recovery/Confirm")]
        [HttpPost]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordConfirmRequest recoveryPasswordConfirmRequest)
        {
            IdentityResult response = await AuthenticationService.RecoveryPasswordConfirmedAsync(recoveryPasswordConfirmRequest);
            if (response.Succeeded)
                return NoContent();
            else
                return BadRequest(new { message = string.Join('\n', response.Errors.Select(x => x.Description)) });
        }

        [Route("Activate/Send")]
        [HttpPost]
        public async Task<IActionResult> ReSendConfirmEmail([FromBody] ReSendEmailConfirmationRequest reSendEmailConfirmationRequest)
        {
            if ((await AuthenticationService.ReSendEmailConfirmationAsync(reSendEmailConfirmationRequest)).Success)
                return NoContent();
            else
                return BadRequest();
        }


        [Route("Activate/Confirm")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailConfirmationRequest confirmEmailConfirmationRequest)
        {
            IdentityResult response = await AuthenticationService.ConfirmEmailConfirmationAsync(confirmEmailConfirmationRequest);
            if (response.Succeeded)
                return NoContent();
            else
                return BadRequest(new { message = string.Join('\n', response.Errors.Select(x => x.Description)) });
        }

        [Route("Logout")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] AccessTokenAuthorizationHeader accessTokenAuthorizationHeader)
        {
            IdentityResult response = await AuthenticationService.LogoutAsync(
                new LogoutRequest() { 
                    Token = accessTokenAuthorizationHeader.TokenValue 
            });
            if (response.Succeeded)
                return NoContent();
            else
                return BadRequest(new { message = string.Join('\n', response.Errors.Select(x => x.Description)) });
        }

    }
}