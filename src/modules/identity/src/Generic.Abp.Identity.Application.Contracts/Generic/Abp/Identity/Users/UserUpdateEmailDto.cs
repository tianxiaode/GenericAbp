using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Identity.Users;

public class UserUpdateEmailDto
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    [DisplayName("DisplayName:Email")]
    public string Value { get; set; }

}