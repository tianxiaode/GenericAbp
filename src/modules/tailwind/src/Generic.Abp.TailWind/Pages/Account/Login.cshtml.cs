using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using Generic.Abp.Tailwind.OpenIddict;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public class LoginModel(
    IAccountAppService accountAppService,
    SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    AbpOpenIddictRequestHelper openIddictRequestHelper)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; } = string.Empty;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; } = string.Empty;

    [BindProperty] public LoginInputModel LoginInput { get; set; } = new();

    public bool EnableLocalLogin { get; set; }

    //TODO: Why there is an ExternalProviders if only the VisibleExternalProviders is used.
    public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; } = new List<ExternalProviderModel>();

    public IEnumerable<ExternalProviderModel> VisibleExternalProviders =>
        ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

    public string? ExternalLoginScheme =>
        IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;


    protected IAuthenticationSchemeProvider SchemeProvider { get; } = schemeProvider;

    protected AbpAccountOptions AccountOptions { get; } = accountOptions.Value;

    //protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; } =
        identityDynamicClaimsPrincipalContributorCache;

    public bool ShowCancelButton { get; set; }
    protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; } = openIddictRequestHelper;


    public virtual async Task<IActionResult> OnGetAsync()
    {
        LoginInput = new LoginInputModel();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);


        if (IsExternalLoginOnly)
        {
            return await OnPostExternalLogin(ExternalProviders.First().AuthenticationScheme);
        }

        var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl ?? "~/");

        if (request == null)
        {
            return Page();
        }

        LoginInput.UserNameOrEmailAddress = request.LoginHint ?? string.Empty;

        //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
        var tenant = request.GetParameter(TenantResolverConsts.DefaultTenantKey)?.ToString();
        if (string.IsNullOrEmpty(tenant))
        {
            return Page();
        }

        CurrentTenant.Change(Guid.Parse(tenant));
        Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync(string? action)
    {
        if (action == "Cancel")
        {
            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl ?? "~/");

            var transaction = HttpContext.GetOpenIddictServerTransaction();
            if (request?.ClientId == null || transaction == null)
            {
                return Redirect("~/");
            }

            transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
            transaction.Request = request;

            var notification = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
            transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!,
                notification);

            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        await CheckLocalLoginAsync();


        ValidateModel();


        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);


        await ReplaceEmailToUsernameOfInputIfNeeds();

        await IdentityOptions.SetAsync();

        var result = await SignInManager.PasswordSignInAsync(
            LoginInput.UserNameOrEmailAddress,
            LoginInput.Password,
            LoginInput.RememberMe,
            true
        );

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = result.ToOpenIddictIdentitySecurityLogAction(),
            UserName = LoginInput.UserNameOrEmailAddress
        });

        if (result.RequiresTwoFactor)
        {
            return await TwoFactorLoginResultAsync();
        }

        if (result.IsLockedOut)
        {
            Error = L["UserLockedOutMessage"];
            return Page();
        }

        if (result.IsNotAllowed)
        {
            Error = L["LoginIsNotAllowed"];
            return Page();
        }

        if (!result.Succeeded)
        {
            Error = L["InvalidUserNameOrPassword"];
            return Page();
        }

        //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
        var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                   await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

        Debug.Assert(user != null, nameof(user) + " != null");

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl ?? "~/", ReturnUrlHash);
    }

    /// <summary>
    /// Override this method to add 2FA for your application.
    /// </summary>
    protected virtual Task<IActionResult> TwoFactorLoginResultAsync()
    {
        throw new NotImplementedException();
    }

    protected virtual async Task<List<ExternalProviderModel>> GetExternalProviders()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();

        return schemes
            .Where(x => x.DisplayName != null || x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName,
                StringComparison.OrdinalIgnoreCase))
            .Select(x => new ExternalProviderModel
            {
                DisplayName = x.DisplayName ?? string.Empty,
                AuthenticationScheme = x.Name
            })
            .ToList();
    }

    public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        if (AccountOptions.WindowsAuthenticationSchemeName == provider)
        {
            return await ProcessWindowsLoginAsync();
        }

        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback",
            values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "",
        string returnUrlHash = "", string? remoteError = null)
    {
        //TODO: Did not implemented Identity Server 4 sample for this method (see ExternalLoginCallback in Quickstart of IDS4 sample)
        /* Also did not implement these:
         * - Logout(string logoutId)
         */

        if (remoteError != null)
        {
            Logger.LogWarning($"External login callback error: {remoteError}");
            return RedirectToPage("./Login");
        }

        await IdentityOptions.SetAsync();
        var query = Request.QueryString;
        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return RedirectToPage("./Login");
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

            return await RedirectSafelyAsync(returnUrl, returnUrlHash);
        }

        //TODO: Handle other cases for result!

        var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ??
                    loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (email.IsNullOrWhiteSpace())
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

        return await RedirectSafelyAsync(returnUrl, returnUrlHash);
    }

    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds()
    {
        if (!ValidationHelper.IsValidEmailAddress(LoginInput.UserNameOrEmailAddress))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);
        if (userByEmail == null)
        {
            return;
        }

        LoginInput.UserNameOrEmailAddress = userByEmail.UserName;
    }

    protected virtual async Task CheckLocalLoginAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
        }
    }

    protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
    {
        var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
        if (result.Succeeded)
        {
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

        return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
    }

    public class LoginInputModel
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [DisplayName("UserNameOrEmailAddress")]
        public string UserNameOrEmailAddress { get; set; } = string.Empty;

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [DisableAuditing]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class ExternalProviderModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string AuthenticationScheme { get; set; } = string.Empty;
    }
}