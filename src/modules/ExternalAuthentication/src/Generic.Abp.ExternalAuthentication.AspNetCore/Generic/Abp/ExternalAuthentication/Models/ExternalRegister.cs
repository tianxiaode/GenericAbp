using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.ExternalAuthentication.Models;

public class ExternalRegister
{
    [Required]
    [DisplayName("ExternalAuthentication:ReturnUrl")]
    public string ReturnUrl { get; set; } = string.Empty;

    [DisplayName("ExternalAuthentication:ReturnUrlHash")]
    [Required]
    public string ReturnUrlHash { get; set; } = string.Empty;

    [DisplayName("ExternalAuthentication:RegisterKey")]
    [Required]
    public string RegisterKey { get; set; } = string.Empty;

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    [DisplayName("ExternalAuthentication:UserName")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    [DisplayName("ExternalAuthentication:EmailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DisableAuditing]
    [DisplayName("ExternalAuthentication:Password")]
    public string Password { get; set; } = string.Empty;
}