using Application.DTOs.Authentication;
using Application.Exceptions;
using Application.Interfaces.Resources;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationRequest>
    {
        private readonly UserManager<AppUser> UserManagerService;
        private readonly IAppResource AppResource;

        public UserRegistrationValidator(UserManager<AppUser> userManagerService, IAppResource appResource)
        {
            UserManagerService = userManagerService;
            AppResource = appResource;
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage(AppResource["ValidationUserRegistrationEmail"]).WhenAsync(async (UserRegistrationRequest, CancellationToken) =>
            {
                AppUser existingUser = await UserManagerService.FindByEmailAsync(UserRegistrationRequest.Email);

                if (existingUser != null)
                    throw new ApiException(string.Format(AppResource["ExceptionInvalidRequestRepeatUser"], UserRegistrationRequest.Email));

                return await Task.FromResult(true);
            });

            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage(AppResource["ValidationUserRegistrationPassword"]);

        }
    }
}
