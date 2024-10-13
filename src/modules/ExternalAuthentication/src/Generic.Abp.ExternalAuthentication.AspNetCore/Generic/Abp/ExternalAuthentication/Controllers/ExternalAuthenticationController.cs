using Generic.Abp.ExternalAuthentication.Localization;
using Generic.Abp.ExternalAuthentication.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.ExternalAuthentication.Controllers
{
    [Area("ExternalAuthentication")]
    public class ExternalAuthenticationController : AbpControllerBase
    {
        protected readonly SignInManager<IdentityUser> SignInManager;
        protected readonly AbpAccountOptions AccountOptions;
        protected readonly IOptions<IdentityOptions> IdentityOptions;
        protected readonly IdentitySecurityLogManager IdentitySecurityLogManager;
        protected readonly IdentityUserManager UserManager;
        protected readonly SettingManager SettingManager;

        protected readonly IdentityDynamicClaimsPrincipalContributorCache
            IdentityDynamicClaimsPrincipalContributorCache;

        public ExternalAuthenticationController(SignInManager<IdentityUser> signInManager,
            IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions,
            IdentitySecurityLogManager identitySecurityLogManager, IdentityUserManager userManager,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            SettingManager settingManager)
        {
            LocalizationResource = typeof(ExternalAuthenticationResource);
            SignInManager = signInManager;
            AccountOptions = accountOptions.Value;
            IdentityOptions = identityOptions;
            IdentitySecurityLogManager = identitySecurityLogManager;
            UserManager = userManager;
            IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
            SettingManager = settingManager;
        }

        [HttpGet]
        [Route("/Account/Login")]
        public virtual async Task<IActionResult> LoginAsync(string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new UserFriendlyException("returnUrl is not provided");
            }

            var provider = Regex.Match(returnUrl, @"provider=(\w+)").Groups[1].Value;
            Logger.LogDebug($"External login: provider={provider}, ReturnUrl={returnUrl}");
            return await OnPostExternalLogin(provider, returnUrl);
        }

        [HttpGet]
        [Route("/external-login-callback")]
        public virtual async Task<IActionResult> ExternalLoginCallbackAsync(string returnUrl = "",
            string? remoteError = null)
        {
            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            await IdentityOptions.SetAsync();
            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");


                // 构造错误的回调 URL
                var errorRedirectUri = await GetRedirectUrlAsync(returnUrl, "LoginInfoUnavailable",
                    "Failed to retrieve external login information.");

                return Redirect(errorRedirectUri); // 重定向到 OIDC 的 redirect_uri
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
                return Redirect(await GetRedirectUrlAsync(returnUrl, "LockedOut", "Your account is locked out."));
            }

            if (result.IsNotAllowed)
            {
                Logger.LogWarning($"External login callback error: user is not allowed!");
                return Redirect(await GetRedirectUrlAsync(returnUrl, "NotAllowed",
                    "Your account is not allowed to login."));
            }

            IdentityUser? user;
            if (result.Succeeded)
            {
                Logger.LogDebug("External login callback succeeded. Signing in user...");
                user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                if (user != null)
                {
                    // Clear the dynamic claims cache.
                    await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
                }

                return Redirect(await GetRedirectUrlAsync(returnUrl));
            }

            //用户不存在
            var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ??
                        loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            if (email.IsNullOrWhiteSpace())
            {
                var prefix =
                    await SettingManager.GetOrNullForCurrentTenantAsync(ExternalAuthenticationSettingNames.NewUser
                        .NewUserPrefix) ?? "NewUser-";
                var newUserEmail = await SettingManager.GetOrNullForCurrentTenantAsync(
                    ExternalAuthenticationSettingNames.NewUser
                        .NewUserEmailSuffix) ?? "@quicktemplate.com";
                email = prefix + Guid.NewGuid().ToString("N") + newUserEmail;
            }

            user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser(GuidGenerator.Create(), email, email, CurrentTenant?.Id);
                var createResult = await UserManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    Logger.LogError($"Error creating user: {email}");
                    // 处理创建用户失败的情况
                    var errorDescription = string.Join("\n ", createResult.Errors.Select(e => e.Description));
                    Logger.LogError($"Error creating user: {errorDescription}");

                    var errorRedirectUri = await GetRedirectUrlAsync(returnUrl, "CreateUserFailed", errorDescription);

                    return Redirect(errorRedirectUri); // 重定向到 OIDC 的 redirect_uri

                    //return RedirectToPage("./Error");
                }
            }

            (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                loginInfo.ProviderDisplayName
            ))).CheckErrors();


            await SignInManager.SignInAsync(user, true, loginInfo.LoginProvider);
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = user.Name
            });

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return Redirect(await GetRedirectUrlAsync(returnUrl));
        }

        [HttpGet]
        [Route("/need-set-password")]
        public virtual async Task<Dictionary<string, bool>> NeedRegisterAsync()
        {
            var currentUser = CurrentUser;
            if (string.IsNullOrEmpty(currentUser?.Email))
            {
                return new Dictionary<string, bool>() { { "need", false } };
            }

            Logger.LogDebug("User is already registered with email: " + currentUser.Email);
            var user = await UserManager.FindByEmailAsync(currentUser.Email);
            if (user != null && string.IsNullOrEmpty(user.PasswordHash))
            {
                Logger.LogDebug($"User found: {user.Email}, {user.PasswordHash}");
                return new Dictionary<string, bool>()
                {
                    { "need", true }
                };
            }

            return new Dictionary<string, bool>() { { "need", false } };
        }


        protected virtual async Task<IActionResult> OnPostExternalLogin(string provider, string returnUrl)
        {
            // 如果是 Windows 认证，走特殊流程
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync(returnUrl);
            }

            // 手动构建指向 /external-login-callback 的回调 URL
            var redirectUrl = Url.Action("ExternalLoginCallback", "AuthenticationController",
                new { returnUrl }, Request.Scheme, Request.Host.ToString());
            // Url.RouteUrl("/external-login-callback", new { returnUrl, returnUrlHash }, Request.Scheme,
            // Request.Host.ToString());

            Logger.LogDebug($"Redirecting to external login provider: {provider}, RedirectUrl={redirectUrl}");
            // 配置 AuthenticationProperties，包含回调 URL
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //
            // // 可以手动添加外部登录方案的名称到 properties
            properties.Items["scheme"] = provider;
            //
            // // 发起外部登录请求并传递指定的回调 URL
            return Challenge(properties, provider);
        }

        protected virtual async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (!result.Succeeded)
            {
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }

            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("ExternalLoginCallback", "AuthenticationController",
                    new { returnUrl }, Request.Scheme, Request.Host.ToString()),
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

        protected virtual async Task<string> GetRedirectUrlAsync(string returnUrl,
            string? error = null, string? errorDescription = null)
        {
            returnUrl = await NormalizeReturnUrlAsync(returnUrl);

            if (!string.IsNullOrEmpty(error))
            {
                returnUrl = QueryHelpers.AddQueryString(returnUrl, new Dictionary<string, string>()
                {
                    { "error", error },
                    { "error_description", errorDescription ?? "" }
                }!);
            }

            return returnUrl;
        }

        protected virtual async Task<string> NormalizeReturnUrlAsync(string returnUrl)
        {
            return returnUrl.IsNullOrEmpty() ? await Task.FromResult("~/") : returnUrl;
        }
    }
}