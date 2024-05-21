using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.UI.Navigation.Urls;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders;

[RemoteService(Name = OAuthProviderManagerRemoteServiceConsts.RemoteServiceName)]
[Area("OAuthProviderManager")]
[Route("api/oauth-providers")]
public class OAuthProviderController : OAuthProviderManagerController, IOAuthProviderAppService
{
    protected readonly IOAuthProviderAppService AppService;
    protected readonly AbpSignInManager SignInManager;
    protected readonly IdentityUserManager UserManager;
    protected readonly IdentitySecurityLogManager IdentitySecurityLogManager;
    protected readonly IOptions<IdentityOptions> IdentityOptions;
    protected readonly IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache;
    //protected IAppUrlProvider AppUrlProvider => LazyServiceProvider.LazyGetRequiredService<IAppUrlProvider>();

    public OAuthProviderController(IOAuthProviderAppService appService, AbpSignInManager signInManager,
        IdentityUserManager userManager, IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    {
        AppService = appService;
        SignInManager = signInManager;
        UserManager = userManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityOptions = identityOptions;
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
    }

    [HttpGet]
    public virtual Task<ListResultDto<OAuthProviderDto>> GetListAsync(OAuthProviderGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{provider}")]
    public virtual Task<OAuthProviderDto> UpdateAsync(string provider, [FromBody] OAuthProviderUpdateDto input)
    {
        return AppService.UpdateAsync(provider, input);
    }

    [HttpPost]
    [Route("/oauth-login")]
    public virtual async Task<IActionResult> OAuthLoginAsync([FromBody] OAuthLoginDto input)
    {
        var provider = input.Provider;
        var redirectUrl = Url.Page("./oauth-login/callback", pageHandler: "ExternalLoginCallback",
            values: new { ReturnUrl = input.ReturnUrl, ReturnUrlHash = input.ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    [HttpGet]
    [Route("/oauth-login/callback")]
    public virtual async Task<IActionResult> OAuthLoginCallbackAsync(string returnUrl = "",
        string returnUrlHash = "", string remoteError = null)
    {
        //TODO: Did not implemented Identity Server 4 sample for this method (see ExternalLoginCallback in Quickstart of IDS4 sample)
        /* Also did not implement these:
         * - Logout(string logoutId)
         */

        if (remoteError != null)
        {
            Logger.LogWarning($"External login callback error: {remoteError}");
            return await RedirectSafely(returnUrl, returnUrlHash, remoteError);
        }

        await IdentityOptions.SetAsync();

        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return await RedirectSafely(returnUrl, returnUrlHash, "External login info is not available");
        }

        var result = await SignInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = "Login" + result
            });
        }

        if (result.IsLockedOut)
        {
            Logger.LogWarning($"External login callback error: user is locked out!");
            //throw new UserFriendlyException("Cannot proceed because user is locked out!");
            return await RedirectSafely(returnUrl, returnUrlHash, "External login callback error: user is locked out!");
        }

        if (result.IsNotAllowed)
        {
            Logger.LogWarning($"External login callback error: user is not allowed!");
            //throw new UserFriendlyException("Cannot proceed because user is not allowed!");
            return await RedirectSafely(returnUrl, returnUrlHash,
                "External login callback error: user is not allowed!");
        }

        IdentityUser user;
        if (result.Succeeded)
        {
            user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user != null)
            {
                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
            }

            return await RedirectSafely(returnUrl, returnUrlHash);
        }

        //TODO: Handle other cases for result!

        var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ??
                    loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToPage("./Register", new
            {
                IsExternalLogin = true,
                ExternalLoginAuthSchema = loginInfo.LoginProvider,
                ReturnUrl = returnUrl
            });
        }

        user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            return RedirectToPage("./Register", new
            {
                IsExternalLogin = true,
                ExternalLoginAuthSchema = loginInfo.LoginProvider,
                ReturnUrl = returnUrl
            });
        }

        if (await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey) == null)
        {
            CheckIdentityErrors(await UserManager.AddLoginAsync(user, loginInfo));
        }

        await SignInManager.SignInAsync(user, false);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = user.Name
        });

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return RedirectSafely(returnUrl, returnUrlHash);
    }

    protected virtual async Task<RedirectResult> RedirectSafely(string returnUrl, string? returnUrlHash = null,
        string errors = null)
    {
        var url = await GetRedirectUrl(returnUrl, returnUrlHash);
        if (errors != null)
        {
            url += "?errors=" + errors;
        }

        return Redirect(url);
    }

    protected virtual async Task<string> GetRedirectUrl(string returnUrl, string? returnUrlHash = null)
    {
        returnUrl = await NormalizeReturnUrl(returnUrl);

        if (!string.IsNullOrWhiteSpace(returnUrlHash))
        {
            returnUrl = returnUrl + returnUrlHash;
        }

        return returnUrl;
    }

    protected virtual async Task<string> NormalizeReturnUrl(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            return GetAppHomeUrl();
        }

        if (Url.IsLocalUrl(returnUrl) || await AppUrlProvider.IsRedirectAllowedUrlAsync(returnUrl))
        {
            return returnUrl;
        }

        return GetAppHomeUrl();
    }

    protected virtual string GetAppHomeUrl()
    {
        return Url.Content("~/");
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " +
                                            identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}")
                                                .JoinAsString(", "));
        }

        //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
    }
}