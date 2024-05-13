using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

//TODO: Implement live password complexity check on the razor view!
public class ResetPasswordModel(
    IAccountAppService accountAppService,
    global::Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
    IdentityUserManager userManager,
    IdentitySecurityLogManager identitySecurityLogManager,
    IOptions<IdentityOptions> identityOptions,
    IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    : AccountPageModel(accountAppService, signInManager, userManager, identitySecurityLogManager, identityOptions,
        exceptionToErrorInfoConverter)
{
    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResetToken { get; set; } = string.Empty;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; } = string.Empty;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DisableAuditing]
    public string Password { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [Compare("Password")]
    [DisableAuditing]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool InvalidToken { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ValidateModel();

        InvalidToken = !await AccountAppService.VerifyPasswordResetTokenAsync(
            new VerifyPasswordResetTokenInput
            {
                UserId = UserId,
                ResetToken = ResetToken
            }
        );


        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ValidateModel();

            await AccountAppService.ResetPasswordAsync(
                new ResetPasswordDto
                {
                    UserId = UserId,
                    ResetToken = ResetToken,
                    Password = Password
                }
            );
        }
        catch (AbpIdentityResultException e)
        {
            if (string.IsNullOrWhiteSpace(e.Message))
            {
                throw;
            }

            Error = GetLocalizeExceptionMessage(e);
            return Page();
        }
        catch (AbpValidationException)
        {
            return Page();
        }

        //TODO: Try to automatically login!
        return RedirectToPage("./ResetPasswordConfirmation", new
        {
            returnUrl = ReturnUrl,
            returnUrlHash = ReturnUrlHash
        });
    }

    protected override void ValidateModel()
    {
        if (!Equals(Password, ConfirmPassword))
        {
            ModelState.AddModelError("ConfirmPassword",
                L["'{0}' and '{1}' do not match.", "ConfirmPassword", "Password"]);
        }

        base.ValidateModel();
    }
}