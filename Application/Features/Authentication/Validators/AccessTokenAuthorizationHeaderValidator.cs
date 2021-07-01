using Application.Binders;
using Application.Interfaces.Resources;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class AccessTokenAuthorizationHeaderValidator : AbstractValidator<AccessTokenAuthorizationHeader>
    {
        private readonly IAppResource AppResource;

        public AccessTokenAuthorizationHeaderValidator(IAppResource appResource)
        {
            AppResource = appResource;
            RuleFor(x => x.TokenValue).NotEmpty().NotNull().WithMessage(AppResource["ValidationLogoutToken"]);
        }
    }
}
