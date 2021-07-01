using Application.DTOs.Authentication;
using Application.Interfaces.Resources;
using FluentValidation;

namespace AdaIntelligenceApi.Domain.Validators
{
    public class RecoveryPasswordValidator : AbstractValidator<RecoveryPasswordRequest>
    {
        private readonly IAppResource AppResource;

        public RecoveryPasswordValidator(IAppResource appResource)
        {
            AppResource = appResource;

            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage(AppResource["ValidationRecoveryPasswordEmail"]);
        }

    }
}
