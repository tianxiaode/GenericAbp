using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public class LogoutModel(
    IAccountAppService accountAppService,
    global::Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; } = string.Empty;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; } = string.Empty;

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.Logout
        });

        await SignInManager.SignOutAsync();
        if (!ReturnUrl.IsNullOrEmpty())
        {
            return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
        }

        if (await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            return RedirectToPage("/Account/Login");
        }

        return RedirectToPage("/");
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}