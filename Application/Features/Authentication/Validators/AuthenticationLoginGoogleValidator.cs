using Application.DTOs.Authentication;
using Application.Interfaces.Resources;
using FluentValidation;

namespace AdaIntelligenceApi.Domain.Validators
{
    public class AuthenticationLoginGoogleValidator : AbstractValidator<AuthenticationLoginGoogleRequest>
    {
        private readonly IAppResource AppResource;

        public AuthenticationLoginGoogleValidator(IAppResource appResource)
        {
            AppResource = appResource;
            RuleFor(x => x.TokenId).NotEmpty().NotNull().WithMessage(AppResource["ExceptionInvalidRequestInvalidToken"]);
        }
    }
}
