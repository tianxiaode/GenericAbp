using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web;
using Volo.Abp.Validation;

namespace Generic.Abp.PhoneLogin.Account.Web.Pages.Account
{
    public class LoginModel : Volo.Abp.Account.Web.Pages.Account.LoginModel
    {
        public LoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions, PhoneLoginUserManager phoneLoginUserManager) : base(schemeProvider, accountOptions, identityOptions)
        {
            PhoneLoginUserManager = phoneLoginUserManager;
        }
        protected PhoneLoginUserManager PhoneLoginUserManager { get; }
        protected override async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {

            var userByUsername = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByPhone = await PhoneLoginUserManager.FindByPhoneAsync(LoginInput.UserNameOrEmailAddress);
            if (userByPhone != null)
            {
                LoginInput.UserNameOrEmailAddress = userByPhone.UserName;
                return;
            }

            if (!ValidationHelper.IsValidEmailAddress(LoginInput.UserNameOrEmailAddress))
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);
            if (userByEmail != null)
            {
                LoginInput.UserNameOrEmailAddress = userByEmail.UserName;
                return;
            }


            return;


        }

    }
}
