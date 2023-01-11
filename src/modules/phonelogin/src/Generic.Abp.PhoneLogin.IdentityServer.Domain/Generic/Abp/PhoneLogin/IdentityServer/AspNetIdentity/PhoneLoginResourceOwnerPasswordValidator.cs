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
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.PhoneLogin.IdentityServer.AspNetIdentity
{
    public class PhoneLoginResourceOwnerPasswordValidator : AbpResourceOwnerPasswordValidator
    {
        public PhoneLoginResourceOwnerPasswordValidator(
            PhoneLoginUserManager userManager,
            SignInManager<IdentityUser> signInManager,
            IdentitySecurityLogManager identitySecurityLogManager,
            ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger,
            IStringLocalizer<AbpIdentityServerResource> localizer,
            IOptions<AbpIdentityOptions> abpIdentityOptions,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<IdentityOptions> identityOptions) :
            base(userManager, signInManager, identitySecurityLogManager, logger, localizer, abpIdentityOptions, serviceScopeFactory, identityOptions)
        {
            PhoneLoginUserManager = userManager;
        }
        protected PhoneLoginUserManager PhoneLoginUserManager { get; }

        protected override async Task ReplaceEmailToUsernameOfInputIfNeeds(ResourceOwnerPasswordValidationContext context)
        {

            var userByUsername = await UserManager.FindByNameAsync(context.UserName);
            if (userByUsername != null)
            {
                return;
            }

            var userByPhone = await PhoneLoginUserManager.FindByPhoneAsync(context.UserName);
            if (userByPhone != null)
            {
                context.UserName = userByPhone.UserName;
                return;
            }

            if (!ValidationHelper.IsValidEmailAddress(context.UserName))
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(context.UserName);
            if (userByEmail == null)
            {
                return;
            }

            context.UserName = userByEmail.UserName;
        }

    }
}
