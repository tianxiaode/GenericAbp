#nullable enable
using Generic.Abp.PhoneLogin.Exceptions;
using Generic.Abp.PhoneLogin.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Generic.Abp.PhoneLogin
{
    public class PhoneLoginUserValidator : IUserValidator<IdentityUser>
    {
        public PhoneLoginUserValidator(PhoneLoginUserManager phoneLoginUserManager,
            IStringLocalizer<PhoneLoginResource> localizer)
        {
            PhoneLoginUserManager = phoneLoginUserManager;
            Localizer = localizer;
        }

        protected PhoneLoginUserManager PhoneLoginUserManager { get; }
        protected IStringLocalizer<PhoneLoginResource> Localizer { get; }

        private async Task<List<IdentityError>?> ValidatePhone(IdentityUser user)
        {
            var phone = await PhoneLoginUserManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new InvalidPhoneNumberBusinessException(phone);
            }

            var owner = await PhoneLoginUserManager.FindByPhoneAsync(phone).ConfigureAwait(false);
            if (owner != null &&
                !string.Equals(await PhoneLoginUserManager.GetUserIdAsync(owner).ConfigureAwait(false),
                    await PhoneLoginUserManager.GetUserIdAsync(user).ConfigureAwait(false)))
            {
                throw new DuplicatePhoneNumberBusinessException(phone);
            }

            return null;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var errors = await ValidatePhone(user).ConfigureAwait(false);
            return errors?.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}