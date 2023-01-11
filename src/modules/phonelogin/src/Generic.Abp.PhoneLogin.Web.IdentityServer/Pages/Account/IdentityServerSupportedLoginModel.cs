using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Generic.Abp.PhoneLogin.Web.Pages.Account
{
    [ExposeServices(typeof(LoginModel))]
    public class IdentityServerSupportedLoginModel : Volo.Abp.Account.Web.Pages.Account.IdentityServerSupportedLoginModel
    {
        protected PhoneLoginUserManager PhoneLoginUserManager { get; }

        public IdentityServerSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService identityServerEvents,
            IOptions<IdentityOptions> identityOptions,
            PhoneLoginUserManager phoneLoginUserManager)
            : base(
                schemeProvider,
                accountOptions,
                interaction,
                clientStore,
                identityServerEvents,
                identityOptions
                )
        {
            PhoneLoginUserManager = phoneLoginUserManager;
        }



    }
}
