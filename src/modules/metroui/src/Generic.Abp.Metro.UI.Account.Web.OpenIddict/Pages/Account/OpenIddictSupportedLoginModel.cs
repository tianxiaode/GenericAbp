using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Generic.Abp.OpenIddict;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;

namespace Generic.Abp.Metro.UI.Account.Web.Pages.Account;

[ExposeServices(typeof(LoginModel))]
public class OpenIddictSupportedLoginModel : LoginModel
{
    protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; }

    public OpenIddictSupportedLoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions,
        AbpOpenIddictRequestHelper openIddictRequestHelper)
        : base(schemeProvider, accountOptions, identityOptions)
    {
        OpenIddictRequestHelper = openIddictRequestHelper;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        LoginInput = new LoginInputModel();

        var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);
        if (request?.ClientId == null) return await base.OnGetAsync();
        // TODO: Find a proper cancel way.
        // ShowCancelButton = true;

        LoginInput.UserNameOrEmailAddress = request.LoginHint;

        //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
        var tenant = request.GetParameter(TenantResolverConsts.DefaultTenantKey)?.ToString();
        if (string.IsNullOrEmpty(tenant)) return await base.OnGetAsync();
        if (CurrentTenant == null) return await base.OnGetAsync();
        CurrentTenant.Change(Guid.Parse(tenant));
        Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);

        return await base.OnGetAsync();
    }

    public override async Task<IActionResult> OnPostAsync(string action)
    {
        if (action != "Cancel") return await base.OnPostAsync(action);
        var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);

        var transaction = HttpContext.GetOpenIddictServerTransaction();
        if (request?.ClientId == null || transaction == null) return Redirect("~/");
        transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
        transaction.Request = request;

        var notification = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
        transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!,
            notification);

        return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    public override async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        if (AccountOptions.WindowsAuthenticationSchemeName == provider)
        {
            return await ProcessWindowsLoginAsync();
        }

        return await base.OnPostExternalLogin(provider);
    }

    protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
    {
        var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
        if (!result.Succeeded) return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        var props = new AuthenticationProperties()
        {
            RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback",
                values: new { ReturnUrl, ReturnUrlHash }),
            Items =
            {
                {
                    "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                }
            }
        };

        var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
        id.AddClaim(new Claim(ClaimTypes.NameIdentifier,
            result.Principal.FindFirstValue(ClaimTypes.PrimarySid) ?? string.Empty));
        id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty));

        await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

        return Redirect(props.RedirectUri!);
    }
}