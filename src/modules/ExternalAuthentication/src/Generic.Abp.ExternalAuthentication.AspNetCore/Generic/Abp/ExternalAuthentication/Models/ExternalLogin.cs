using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.ExternalAuthentication.Models;

[Serializable]
public class ExternalLogin
{
    [Required]
    [DisplayName("ExternalAuthentication:Provider")]
    public string Provider { get; set; } = string.Empty;

    [Required]
    [DisplayName("ExternalAuthentication:ReturnUrl")]
    public string ReturnUrl { get; set; } = string.Empty;

    [Required]
    [DisplayName("ExternalAuthentication:ReturnUrlHash")]
    public string ReturnUrlHash { get; set; } = string.Empty;
}