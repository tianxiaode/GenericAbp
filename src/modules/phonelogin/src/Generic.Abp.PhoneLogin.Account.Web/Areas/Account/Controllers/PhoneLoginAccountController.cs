using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Areas.Account.Controllers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using UserLoginInfo = Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo;

namespace Generic.Abp.PhoneLogin.Account.Web.Areas.Account.Controllers
{
    [IgnoreAntiforgeryToken]
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Controller]
    [ControllerName("Login")]
    [Area("account")]
    [Route("api/account")]
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(AccountController), IncludeSelf = true)]
    public class PhoneLoginAccountController : AccountController
    {
        public PhoneLoginAccountController(
            SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager,
            ISettingProvider settingProvider,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            PhoneLoginUserManager phoneLoginUserManager) :
        base(signInManager, userManager, settingProvider, identitySecurityLogManager, identityOptions)
        {
            LocalizationResource = typeof(AccountResource);
            PhoneLoginUserManager = phoneLoginUserManager;
        }

        protected PhoneLoginUserManager PhoneLoginUserManager { get; }


        protected override async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
        {

            var userByUsername = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByPhone = await PhoneLoginUserManager.FindByPhoneAsync(login.UserNameOrEmailAddress);
            if (userByPhone != null)
            {
                login.UserNameOrEmailAddress = userByPhone.UserName;
                return;
            }

            if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail != null)
            {
                login.UserNameOrEmailAddress = userByEmail.UserName;
                return;
            }


            return;

        }


    }
}
