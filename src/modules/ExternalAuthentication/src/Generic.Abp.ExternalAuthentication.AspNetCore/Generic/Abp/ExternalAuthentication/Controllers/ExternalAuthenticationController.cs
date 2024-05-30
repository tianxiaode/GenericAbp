using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Generic.Abp.ExternalAuthentication.Models;
using Generic.Abp.ExternalAuthentication.Permissions;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
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
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    IDistributedCache<ExternalRegisterCacheItem> cache)
    : AbpController
{
    protected readonly IAuthenticationSchemeProvider SchemeProvider = schemeProvider;
    protected readonly ISettingManager SettingManager = settingManager;
    protected readonly SignInManager<IdentityUser> SignInManager = signInManager;
    protected readonly IOptions<IdentityOptions> IdentityOptions = identityOptions;
    protected readonly IdentitySecurityLogManager IdentitySecurityLogManager = identitySecurityLogManager;
    protected readonly IdentityUserManager UserManager = userManager;
    protected readonly IDistributedCache<ExternalRegisterCacheItem> Cache = cache;


    protected readonly IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache =
        identityDynamicClaimsPrincipalContributorCache;

    private const string SettingPrefix = "ExternalAuthenticationProvider.";

    [HttpGet]
    [Route("/external-providers")]
    public virtual async Task<ListResultDto<ExternalProvider>> GetListAsync(ExternalProviderGetListInput input)
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var result = new List<ExternalProvider>();
        foreach (var scheme in schemes.Where(x => x.DisplayName != null))
        {
            var enabledString = "";
            try
            {
                Logger.LogDebug($"Get setting for {scheme.Name}");
                enabledString = await SettingManager.GetOrNullForCurrentTenantAsync($"{SettingPrefix}${scheme.Name}");
            }
            catch
            {
                // ignored
            }

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
    [Route("/external-providers")]
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

    [HttpGet]
    [Route("/external-login")]
    public virtual async Task<IActionResult> ExternalLoginAsync(ExternalLogin input)
    {
        var provider = input.Provider;
        var redirectUrl = Url.Action("ExternalLoginCallback", "ExternalAuthentication",
            new { input.ReturnUrl, input.ReturnUrlHash }, protocol: Request.Scheme);
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
        }

        //TODO: Handle other cases for result!

        var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ??
                    loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (email.IsNullOrWhiteSpace())
        {
            //重新定向到returnUrl + 注册页面
            return await RedirectRegisterUrlAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, returnUrl,
                returnUrlHash);
        }

        user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            //重新定向到returnUrl + 注册页面
            return await RedirectRegisterUrlAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, returnUrl,
                returnUrlHash, email);
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

        // Generate authorization code
        var principal = await SignInManager.CreateUserPrincipalAsync(user);
        var properties = new AuthenticationProperties
        {
            RedirectUri = returnUrl
        };

        // 使用 OpenIddict 生成授权码
        return SignIn(principal, properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //var a = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        //// 构建包含授权码的返回 URL
        //var returnUrlWithCode = QueryHelpers.AddQueryString(returnUrl, "code", a.Principa);
        //return await RedirectSafelyAsync(returnUrl, returnUrlHash);
    }

    [HttpPost]
    [Route("/external-register")]
    public virtual async Task ExternalRegisterAsync([FromBody] ExternalRegister input)
    {
        var providerInfo = await Cache.GetAsync(input.RegisterKey);
        if (providerInfo == null)
        {
            Logger.LogWarning("External provider info is not available");
            throw new UserFriendlyException("Cannot proceed because provider login info is not available!");
        }

        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var scheme = schemes.FirstOrDefault(m =>
            string.Equals(m.Name.ToLower(), providerInfo.Provider.ToLower(), StringComparison.CurrentCulture));
        if (scheme == null)
        {
            Logger.LogWarning("External scheme info is not available");
            throw new UserFriendlyException("Cannot proceed because scheme login info is not available!");
        }

        if (input.UserName.IsNullOrWhiteSpace())
        {
            input.UserName = await GetUserNameFromEmailAsync(input.EmailAddress);
        }

        await RegisterExternalUserAsync(providerInfo, input.UserName, input.Password, input.EmailAddress,
            scheme.Name, input.ReturnUrl, input.ReturnUrlHash);
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

    protected virtual async Task<string> GetUserNameFromEmailAsync(string email)
    {
        var userName = email.Split('@')[0];
        var existUser = await UserManager.FindByNameAsync(userName);
        while (existUser != null)
        {
            var randomUserName = userName + RandomHelper.GetRandom(1000, 9999);
            existUser = await UserManager.FindByNameAsync(randomUserName);
            if (existUser != null)
            {
                continue;
            }

            userName = randomUserName;
            break;
        }

        return userName;
    }


    protected virtual async Task RegisterExternalUserAsync(ExternalRegisterCacheItem providerInfo,
        string userName,
        string password,
        string emailAddress, string externalLoginAuthSchema, string returnUrl, string returnUrlHash)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        (await UserManager.CreateAsync(user, password)).CheckErrors();
        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();


        var userLoginAlreadyExists = user.Logins.Any(x =>
            x.TenantId == user.TenantId &&
            x.LoginProvider == providerInfo.Provider &&
            x.ProviderKey == providerInfo.ProviderKey);

        if (!userLoginAlreadyExists)
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                providerInfo.Provider,
                providerInfo.ProviderKey,
                null
            ))).CheckErrors();
        }

        //var signInResult =
        //    await SignInManager.PasswordSignInAsync(user.UserName, password, isPersistent: true,
        //        lockoutOnFailure: false);
        //if (!signInResult.Succeeded)
        //{
        //    throw new UserFriendlyException("Cannot proceed because user is not allowed!");
        //}

        //await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        // Clear the dynamic claims cache.
    }


    protected virtual async Task<RedirectResult> RedirectRegisterUrlAsync(string provider, string providerKey,
        string returnUrl,
        string? returnUrlHash, string? email = null)
    {
        var externalRegisterKey = Guid.NewGuid().ToString().Replace("-", string.Empty);
        ;
        await Cache.SetAsync(externalRegisterKey, new ExternalRegisterCacheItem(provider, providerKey),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        var userName = email != null ? await GetUserNameFromEmailAsync(email) : "";
        var queryParams = new Dictionary<string, string?>
        {
            { "registerKey", externalRegisterKey },
            { "token", await GenerateJwt(externalRegisterKey, email, userName) },
            { "returnUrlHash", returnUrlHash }
        };
        var returnUrlWithParams = QueryHelpers.AddQueryString(returnUrl, queryParams!);
        return Redirect(returnUrlWithParams);
        //return await RedirectSafelyAsync(returnUrlWithParams, returnUrlHash);
    }


    protected virtual Task<string> GenerateJwt(string registerKey, string? email, string? userName)
    {
        var claims = new[]
        {
            new Claim("email", email ?? ""),
            new Claim("userName", userName ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(registerKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourIssuer",
            audience: "yourAudience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}