using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public class RegisterModel(
    IAccountAppService accountAppService,
    SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
    IAuthenticationSchemeProvider schemeProvider,
    IOptions<AbpAccountOptions> accountOptions,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public string ReturnUrlHash { get; set; } = string.Empty;

    [BindProperty] public PostInput Input { get; set; } = new();

    [BindProperty(SupportsGet = true)] public bool IsExternalLogin { get; set; }

    [BindProperty(SupportsGet = true)] public string ExternalLoginAuthSchema { get; set; } = string.Empty;

    public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; } = new List<ExternalProviderModel>();

    public IEnumerable<ExternalProviderModel> VisibleExternalProviders =>
        ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool EnableLocalRegister { get; set; }
    public bool IsExternalLoginOnly => EnableLocalRegister == false && ExternalProviders?.Count() == 1;

    public string? ExternalLoginScheme =>
        IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    protected IAuthenticationSchemeProvider SchemeProvider { get; } = schemeProvider;

    protected AbpAccountOptions AccountOptions { get; } = accountOptions.Value;

    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; } =
        identityDynamicClaimsPrincipalContributorCache;

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ExternalProviders = await GetExternalProviders();

        if (!await CheckSelfRegistrationAsync())
        {
            if (IsExternalLoginOnly && ExternalLoginScheme != null)
            {
                return await OnPostExternalLogin(ExternalLoginScheme);
            }

            Alerts.Warning(L["SelfRegistrationDisabledMessage"]);
        }

        await TrySetEmailAsync();

        return Page();
    }

    protected virtual async Task TrySetEmailAsync()
    {
        if (IsExternalLogin)
        {
            var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return;
            }

            if (!externalLoginInfo.Principal.Identities.Any())
            {
                return;
            }

            var identity = externalLoginInfo.Principal.Identities.First();
            var emailClaim = identity.FindFirst(AbpClaimTypes.Email) ?? identity.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                return;
            }

            var userName = await GetUserNameFromEmail(emailClaim.Value);
            Input = new PostInput { UserName = userName, EmailAddress = emailClaim.Value };
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ExternalProviders = await GetExternalProviders();

            if (!await CheckSelfRegistrationAsync())
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }

            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    Logger.LogWarning("External login info is not available");
                    return RedirectToPage("./Login");
                }

                if (Input.UserName.IsNullOrWhiteSpace())
                {
                    Input.UserName = await GetUserNameFromEmail(Input.EmailAddress);
                }

                await RegisterExternalUserAsync(externalLoginInfo, Input.UserName, Input.EmailAddress);
            }
            else
            {
                await RegisterLocalUserAsync();
            }

            return
                Redirect(ReturnUrl ??
                         "~/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        }
        catch (BusinessException e)
        {
            Alerts.Danger(GetLocalizeExceptionMessage(e) ?? string.Empty);
            return Page();
        }
    }

    protected virtual async Task RegisterLocalUserAsync()
    {
        ValidateModel();

        var userDto = await AccountAppService.RegisterAsync(
            new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName
            }
        );

        var user = await UserManager.GetByIdAsync(userDto.Id);
        await SignInManager.SignInAsync(user, isPersistent: true);

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
    }

    protected virtual async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string userName,
        string emailAddress)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        (await UserManager.CreateAsync(user)).CheckErrors();
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

        await SignInManager.SignInAsync(user, isPersistent: true, ExternalLoginAuthSchema);

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
    }

    protected virtual async Task<bool> CheckSelfRegistrationAsync()
    {
        EnableLocalRegister = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin) &&
                              await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled);

        if (IsExternalLogin)
        {
            return true;
        }

        if (!EnableLocalRegister)
        {
            return false;
        }

        return true;
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

    protected virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback",
            values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    public class PostInput
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ExternalProviderModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string AuthenticationScheme { get; set; } = string.Empty;
    }
}