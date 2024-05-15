using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public class ForgotPasswordModel(
    IAccountAppService accountAppService,
    SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await AccountAppService.SendPasswordResetCodeAsync(
                new SendPasswordResetCodeDto
                {
                    Email = Email,
                    AppName = "MVC", //TODO: Const!
                    ReturnUrl = ReturnUrl,
                    ReturnUrlHash = ReturnUrlHash
                }
            );
        }
        catch (UserFriendlyException e)
        {
            Error = GetLocalizeExceptionMessage(e);
            return Page();
        }


        return RedirectToPage(
            "./PasswordResetLinkSent",
            new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
    }
}