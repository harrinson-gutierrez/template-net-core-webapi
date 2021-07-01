using Application.DTOs.Authentication;
using Application.Interfaces.Resources;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class AuthenticationLoginFacebookValidator : AbstractValidator<AuthenticationLoginFacebookRequest>
    {
        private readonly IAppResource AppResource;

        public AuthenticationLoginFacebookValidator(IAppResource appResource)
        {
            AppResource = appResource;
            RuleFor(x => x.AccessToken).NotEmpty().NotNull().WithMessage(AppResource["ExceptionInvalidRequestInvalidToken"]);
        }
    }
}
