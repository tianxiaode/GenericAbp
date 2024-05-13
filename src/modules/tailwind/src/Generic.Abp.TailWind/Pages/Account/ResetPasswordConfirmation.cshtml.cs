using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

[AllowAnonymous]
public class ResetPasswordConfirmationModel(
    IAccountAppService accountAppService,
    SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public string ReturnUrlHash { get; set; } = string.Empty;

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ReturnUrl = await GetRedirectUrlAsync(ReturnUrl, ReturnUrlHash);

        return Page();
    }
}