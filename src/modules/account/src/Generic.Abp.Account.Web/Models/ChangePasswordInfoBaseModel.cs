using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Account.Web.Models
{
    public class ChangePasswordInfoBaseModel
    {

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "DisplayName:NewPassword")]
        [DisableAuditing]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "DisplayName:NewPasswordConfirm")]
        [DisableAuditing]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }

    }
}
