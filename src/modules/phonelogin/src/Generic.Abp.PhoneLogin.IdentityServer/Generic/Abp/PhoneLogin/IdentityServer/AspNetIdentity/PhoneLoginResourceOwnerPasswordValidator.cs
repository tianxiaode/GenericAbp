using IdentityServer4.AspNetIdentity;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Validation;

namespace Generic.Abp.PhoneLogin.IdentityServer.AspNetIdentity
{
    public class PhoneLoginResourceOwnerPasswordValidator : AbpResourceOwnerPasswordValidator
    {
        public PhoneLoginResourceOwnerPasswordValidator(
            PhoneLoginUserManager userManager,
            SignInManager<Volo.Abp.Identity.IdentityUser> signInManager,
            IdentitySecurityLogManager identitySecurityLogManager,
            ILogger<ResourceOwnerPasswordValidator<Volo.Abp.Identity.IdentityUser>> logger,
            IStringLocalizer<AbpIdentityServerResource> localizer,
            IOptions<AbpIdentityOptions> abpIdentityOptions,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<IdentityOptions> identityOptions) :
           base(
               userManager, signInManager, identitySecurityLogManager, logger, localizer, abpIdentityOptions, serviceScopeFactory, identityOptions)
        {
            PhoneLoginUserManager = userManager;
        }

        protected PhoneLoginUserManager PhoneLoginUserManager { get; }
        protected override async Task ReplaceEmailToUsernameOfInputIfNeeds(ResourceOwnerPasswordValidationContext context)
        {
            if (!ValidationHelper.IsValidEmailAddress(context.UserName))
            {
                return;
            }

            var userByUsername = await UserManager.FindByNameAsync(context.UserName);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(context.UserName);
            if (userByEmail != null)
            {
                context.UserName = userByEmail.UserName;
                return;
            }

            var userByPhone = await PhoneLoginUserManager.FindByPhoneAsync(context.UserName);
            if (userByPhone != null)
            {
                context.UserName = userByPhone.PhoneNumber;
                return;
            }

            return;


        }

    }
}
