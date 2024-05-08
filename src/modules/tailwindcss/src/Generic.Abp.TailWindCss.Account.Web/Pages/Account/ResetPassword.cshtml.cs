using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Validation;

namespace Generic.Abp.TailWindCss.Account.Web.Pages.Account;

//TODO: Implement live password complexity check on the razor view!
public class ResetPasswordModel : AccountPageModel
{
    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResetToken { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DisableAuditing]
    public string Password { get; set; }

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [Compare("Password")]
    [DisableAuditing]
    public string ConfirmPassword { get; set; }

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
        catch (AbpValidationException e)
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