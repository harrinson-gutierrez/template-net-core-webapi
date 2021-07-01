using Application.DTOs.Authentication;
using Application.Interfaces.Resources;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class RecoveryPasswordConfirmValidator : AbstractValidator<RecoveryPasswordConfirmRequest>
    {
        private readonly IAppResource AppResource;

        public RecoveryPasswordConfirmValidator(IAppResource appResource)
        {
            AppResource = appResource;

            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage(AppResource["ValidationRecoveryPasswordConfirmEmail"]);
            RuleFor(x => x.Token).NotEmpty().NotNull().WithMessage(AppResource["ValidationRecoveryPasswordConfirmToken"]);
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage(AppResource["ValidationRecoveryPasswordConfirmPassword"]);
        }
    }
}
