using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public class LoggedOutModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ClientName { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? SignOutIframeUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? PostLogoutRedirectUri { get; set; }

    public LoggedOutModel(IAccountAppService accountAppService,
        global::Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
        IdentityUserManager userManager, IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter) : base(
        accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
    {
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await NormalizeUrlAsync();
        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await NormalizeUrlAsync();
        return Page();
    }

    protected virtual async Task NormalizeUrlAsync()
    {
        if (!PostLogoutRedirectUri.IsNullOrWhiteSpace())
        {
            PostLogoutRedirectUri = Url.Content(await GetRedirectUrlAsync(PostLogoutRedirectUri));
        }

        if (!SignOutIframeUrl.IsNullOrWhiteSpace())
        {
            SignOutIframeUrl = Url.Content(await GetRedirectUrlAsync(SignOutIframeUrl));
        }
    }
}