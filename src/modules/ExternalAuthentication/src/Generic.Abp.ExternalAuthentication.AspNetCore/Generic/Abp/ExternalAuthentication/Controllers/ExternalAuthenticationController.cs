using System.Security.Claims;
using Generic.Abp.ExternalAuthentication.Models;
using Generic.Abp.ExternalAuthentication.Permissions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.ExternalAuthentication.Controllers;

[Area("external-login")]
public class ExternalAuthenticationController(
    IAuthenticationSchemeProvider schemeProvider,
    ISettingManager settingManager,
    SignInManager<IdentityUser> signInManager,
    IOptions<IdentityOptions> identityOptions,
    IdentitySecurityLogManager identitySecurityLogManager,
    IdentityUserManager userManager,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    : AbpController
{
    protected readonly IAuthenticationSchemeProvider SchemeProvider = schemeProvider;
    protected readonly ISettingManager SettingManager = settingManager;
    protected readonly SignInManager<IdentityUser> SignInManager = signInManager;
    protected readonly IOptions<IdentityOptions> IdentityOptions = identityOptions;
    protected readonly IdentitySecurityLogManager IdentitySecurityLogManager = identitySecurityLogManager;
    protected readonly IdentityUserManager UserManager = userManager;

    protected readonly IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache =
        identityDynamicClaimsPrincipalContributorCache;

    private const string SettingPrefix = "ExternalAuthenticationProvider.";

    [HttpGet]
    [Route("/external-provider")]
    public virtual async Task<ListResultDto<ExternalProvider>> GetListAsync(ExternalProviderGetListInput input)
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var result = new List<ExternalProvider>();
        foreach (var scheme in schemes)
        {
            var enabledString = await SettingManager.GetOrNullForCurrentTenantAsync($"{SettingPrefix}${scheme.Name}");
            var enabled = true;
            if (!enabledString.IsNullOrWhiteSpace())
            {
                enabled = enabledString == "true";
            }

            result.Add(new ExternalProvider()
            {
                Provider = scheme.Name,
                DisplayName = scheme.DisplayName ?? scheme.Name,
                Enabled = enabled
            });
        }

        result = result.WhereIf(input.OnlyEnabled, m => m.Enabled).ToList();

        return new ListResultDto<ExternalProvider>(result);
    }

    [Authorize(ExternalAuthenticationPermissions.ExternalAuthenticationProviders.ManagePermissions)]
    [HttpPost]
    [Route("/external-provider")]
    public virtual async Task<ExternalProvider> UpdateAsync(string provider, ExternalProviderUpdate input)
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var scheme =
            schemes.FirstOrDefault(m => m.Name.Equals(provider, StringComparison.InvariantCultureIgnoreCase));
        if (scheme == null)
        {
            throw new EntityNotFoundException("{0}");
        }

        await SettingManager.SetForCurrentTenantAsync($"{SettingPrefix}${scheme.Name}", input.Enabled.ToString(), true);
        return new ExternalProvider()
        {
            Provider = scheme.Name,
            DisplayName = scheme.DisplayName ?? scheme.Name,
            Enabled = input.Enabled
        };
    }

    [HttpPost]
    [Route("/external-login")]
    public virtual async Task<IActionResult> ExternalLoginAsync(ExternalLogin input)
    {
        var provider = input.Provider;
        var redirectUrl =
            Url.RouteUrl("/external-login-callback", values: new { input.ReturnUrl, input.ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    [HttpGet]
    [Route("/external-login-callback")]
    public virtual async Task<IActionResult> ExternalLoginCallbackAsync(string returnUrl, string returnUrlHash,
        string? remoteError = null)
    {
        if (remoteError != null)
        {
            Logger.LogWarning($"External login callback error: {remoteError}");
            return RedirectToPage(returnUrl ?? "");
        }

        if (returnUrl == null)
        {
            throw new UserFriendlyException("ReturnUrl is null");
        }

        var url = await GetRedirectUrlAsync(returnUrl, returnUrlHash);

        await IdentityOptions.SetAsync();
        var query = Request.QueryString;
        Logger.LogInformation($"Query string: {query}");
        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return RedirectToPage(returnUrl ?? "");
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
            throw new UserFriendlyException("Cannot proceed because user is locked out!");
        }

        if (result.IsNotAllowed)
        {
            Logger.LogWarning($"External login callback error: user is not allowed!");
            throw new UserFriendlyException("Cannot proceed because user is not allowed!");
        }

        IdentityUser? user;
        if (result.Succeeded)
        {
            user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user != null)
            {
                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
            }

            return Redirect(url);
        }

        //TODO: Handle other cases for result!

        var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ??
                    loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (email.IsNullOrWhiteSpace())
        {
            //重新定向到returnUrl + 注册页面
            return Redirect(await GetRegisterRedirectUrlAsync(returnUrl, loginInfo.LoginProvider));
        }

        user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            //重新定向到returnUrl + 注册页面
            return Redirect(await GetRegisterRedirectUrlAsync(returnUrl, loginInfo.LoginProvider));
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

        return await RedirectSafelyAsync(returnUrl, returnUrlHash);
    }

    [HttpPost]
    [Route("/external-register")]
    public virtual async Task RegisterAsync(ExternalRegister input)
    {
        var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            throw new UserFriendlyException("Cannot proceed because external login info is not available!");
        }

        if (input.UserName.IsNullOrWhiteSpace())
        {
            input.UserName = await GetUserNameFromEmail(input.EmailAddress);
        }

        await RegisterExternalUserAsync(externalLoginInfo, input.UserName, input.Password, input.EmailAddress,
            input.Provider);
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

    protected virtual async Task<string> GetUserNameFromEmail(string email)
    {
        var userName = email.Split('@')[0];
        var existUser = await UserManager.FindByNameAsync(userName);
        while (existUser != null)
        {
            var randomUserName = userName + RandomHelper.GetRandom(1000, 9999);
            existUser = await UserManager.FindByNameAsync(randomUserName);
            if (existUser == null)
            {
                userName = randomUserName;
                break;
            }
        }

        return userName;
    }


    protected virtual async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string userName,
        string password,
        string emailAddress, string externalLoginAuthSchema)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        (await UserManager.CreateAsync(user, password)).CheckErrors();
        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();


        var userLoginAlreadyExists = user.Logins.Any(x =>
            x.TenantId == user.TenantId &&
            x.LoginProvider == externalLoginInfo.LoginProvider &&
            x.ProviderKey == externalLoginInfo.ProviderKey);

        if (!userLoginAlreadyExists)
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey,
                externalLoginInfo.ProviderDisplayName
            ))).CheckErrors();
        }

        await SignInManager.SignInAsync(user, isPersistent: true, externalLoginAuthSchema);

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
    }


    protected virtual async Task<string> GetRegisterRedirectUrlAsync(string returnUrl, string provider)
    {
        if (!returnUrl.Contains("?"))
        {
            returnUrl += "?";
        }

        returnUrl += "register=true";
        return await Task.FromResult(returnUrl);
    }
}