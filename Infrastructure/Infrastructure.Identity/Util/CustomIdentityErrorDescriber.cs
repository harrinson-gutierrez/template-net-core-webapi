using Application.Interfaces.Resources;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Util
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IAppResource AppResource;

        public CustomIdentityErrorDescriber(IAppResource appResource)
        {
            AppResource = appResource;
        }

        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = AppResource["IdentityErrorDefaultError"] }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = AppResource["IdentityErrorDefaultError"] }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = AppResource["IdentityErrorPasswordMismatch"] }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = AppResource["InvalidToken"] }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = AppResource["LoginAlreadyAssociated"] }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = AppResource["InvalidUserName", new[] {userName}] }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = AppResource["InvalidEmail", new[] { email }] }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = AppResource["DuplicateUserName", new[] { userName }] }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = AppResource["DuplicateEmail", new[] { email }] }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = AppResource["InvalidRoleName", new[] { role }] }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = AppResource["DuplicateRoleName", new[] { role }] }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = AppResource["UserAlreadyHasPassword"] }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = AppResource["UserLockoutNotEnabled"] }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = AppResource["UserAlreadyInRole", new[] { role }] }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = AppResource["UserNotInRole", new[] { role }] }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = AppResource["PasswordTooShort", new[] { length.ToString() }] }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = AppResource["PasswordRequiresNonAlphanumeric"] }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = AppResource["PasswordRequiresDigit"] }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = AppResource["PasswordRequiresLower"] }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = AppResource["PasswordRequiresUpper"] }; }
    }
}
