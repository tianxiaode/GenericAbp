using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.Identity.Users;

[Serializable]
public class PasswordPolicyUpdateDto
{
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequiredLength")]
    [Range(6,128)]

    public int RequiredLength { get; set; }

    // [Required]
    // [DisplayName("DisplayName:Abp.Identity.Password.RequiredUniqueChars")]
    // [Range(0,int.MaxValue)]
    // public int RequiredUniqueChars { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireNonAlphanumeric")]
    public bool RequireNonAlphanumeric { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireLowercase")]
    public bool RequireLowercase { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireUppercase")]
    public bool RequireUppercase { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireDigit")]
    public bool RequireDigit { get; set; }

}