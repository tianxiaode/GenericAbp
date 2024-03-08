using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Identity.Users;

public class UserUpdateDto: IdentityUserUpdateDto
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DisplayName("DisplayName:ConfirmPassword")]
    public string ConfirmPassword { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Password))
        {
            yield break;
        }

        if (ConfirmPassword == Password)
        {
            yield break;
        }

        yield return new ValidationResult("PasswordNoEqual", new[] {"Password"});

    }
}