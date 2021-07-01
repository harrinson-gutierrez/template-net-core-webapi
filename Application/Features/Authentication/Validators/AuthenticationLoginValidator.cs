using Application.DTOs.Authentication;
using Application.Enums;
using Application.Interfaces.Resources;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class AuthenticationLoginValidator : AbstractValidator<AuthenticationLoginRequest>
    {
        private readonly IAppResource AppResource;

        public AuthenticationLoginValidator(IAppResource appResource)
        {
            AppResource = appResource;

            When(x => x.GrantType.Equals(GrantType.Password), () =>
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage(AppResource["ValidationAuthenticationLoginUsername"])
                                     .NotNull().WithMessage(AppResource["ValidationAuthenticationLoginUsername"]);

                RuleFor(x => x.Password).NotEmpty().WithMessage(AppResource["ValidationAuthenticationLoginPassword"])
                                        .NotNull().WithMessage(AppResource["ValidationAuthenticationLoginPassword"]);
            });

            When(x => x.GrantType.Equals(GrantType.RefreshToken), () =>
            {
                RuleFor(x => x.AccessToken).NotEmpty().NotNull().WithMessage(AppResource["ValidationAuthenticationLoginAccessToken"]);
                RuleFor(x => x.RefreshToken).NotEmpty().NotNull().WithMessage(AppResource["ValidationAuthenticationLoginRefreshToken"]);
            });
        }
    }
}
