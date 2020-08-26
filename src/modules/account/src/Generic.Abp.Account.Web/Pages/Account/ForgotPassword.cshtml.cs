using Generic.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Account.Web.Pages.Account
{

    public class ForgotPasswordModel : AccountPageModel
    {

        //[BindProperty]
        public ForgotPasswordInfoModel ForgotPasswordInfoModel { get; set; }

        public ResetPasswordInfoBaseModel ResetPasswordInfoBaseModel { get; set; }


        public virtual Task<IActionResult> OnGetAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());

        }
    }

    public class ResetPasswordInfoBaseModel : ChangePasswordInfoBaseModel
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
    }
    public class ForgotPasswordInfoModel
    {

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(6)]
        public string Code { get; set; }
    }
}
