using System.ComponentModel;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Identity.Users;

public class UserUpdateSurnameDto
{
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
    [DisplayName("DisplayName:Surname")]
    public string Value { get; set; }

}