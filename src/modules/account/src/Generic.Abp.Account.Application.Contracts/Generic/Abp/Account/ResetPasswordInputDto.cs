using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Account
{
    public class ResetPasswordInputDto
    {
        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(256)]
        public string Token { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "DisplayName:NewPassword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "DisplayName:NewPasswordConfirm")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }

    }
}
