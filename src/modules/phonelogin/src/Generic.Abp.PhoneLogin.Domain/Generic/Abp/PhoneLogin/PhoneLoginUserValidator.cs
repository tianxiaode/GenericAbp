using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Generic.Abp.PhoneLogin
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(UserValidator<IdentityUser>))]
    public class PhoneLoginUserValidator : IUserValidator<IdentityUser>
    {
        public PhoneLoginUserValidator(IdentityErrorDescriber? errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }

        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = await ValidateUserName(manager as PhoneLoginUserManager, user).ConfigureAwait(false);
            if (manager.Options.User.RequireUniqueEmail)
            {
                errors = await ValidateEmail(manager as PhoneLoginUserManager, user, errors).ConfigureAwait(false);
            }
            errors = await ValidatePhone(manager as PhoneLoginUserManager, user, errors).ConfigureAwait(false);
            return errors?.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        private async Task<List<IdentityError>?> ValidateUserName(PhoneLoginUserManager manager, IdentityUser user)
        {
            List<IdentityError>? errors = null;
            var userName = await manager.GetUserNameAsync(user).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors ??= new List<IdentityError>();
                errors.Add(Describer.InvalidUserName(userName));
            }
            else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
            {
                errors ??= new List<IdentityError>();
                errors.Add(Describer.InvalidUserName(userName));
            }
            else
            {
                var owner = await manager.FindByNameAsync(userName).ConfigureAwait(false);
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner).ConfigureAwait(false), await manager.GetUserIdAsync(user).ConfigureAwait(false)))
                {
                    errors ??= new List<IdentityError>();
                    errors.Add(Describer.DuplicateUserName(userName));
                }
            }

            return errors;
        }

        // make sure email is not empty, valid, and unique
        private async Task<List<IdentityError>?> ValidateEmail(PhoneLoginUserManager manager, IdentityUser user, List<IdentityError>? errors)
        {
            var email = await manager.GetEmailAsync(user).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(email))
            {
                errors ??= new List<IdentityError>();
                errors.Add(Describer.InvalidEmail(email));
                return errors;
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                errors ??= new List<IdentityError>();
                errors.Add(Describer.InvalidEmail(email));
                return errors;
            }
            var owner = await manager.FindByEmailAsync(email).ConfigureAwait(false);
            if (owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner).ConfigureAwait(false), await manager.GetUserIdAsync(user).ConfigureAwait(false)))
            {
                errors ??= new List<IdentityError>();
                errors.Add(Describer.DuplicateEmail(email));
            }
            return errors;
        }

        private async Task<List<IdentityError>?> ValidatePhone(PhoneLoginUserManager manager, IdentityUser user, List<IdentityError>? errors)
        {
            var phone = await manager.GetPhoneNumberAsync(user).ConfigureAwait(false);
            var invalidPhoneNumber = new IdentityError()
            {
                Code = "InvalidPhoneNumber"
            };
            if (string.IsNullOrWhiteSpace(phone))
            {
                errors ??= new List<IdentityError>();
                errors.Add(invalidPhoneNumber);
                return errors;
            }
            if (!new EmailAddressAttribute().IsValid(phone))
            {
                errors ??= new List<IdentityError>();
                errors.Add(invalidPhoneNumber);
                return errors;
            }
            var owner = await manager.FindByEmailAsync(phone).ConfigureAwait(false);
            if (owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner).ConfigureAwait(false), await manager.GetUserIdAsync(user).ConfigureAwait(false)))
            {
                errors ??= new List<IdentityError>();
                errors.Add(new IdentityError()
                {
                    Code = "DuplicatePhoneNumber"
                });
            }
            return errors;
        }

    }
}
