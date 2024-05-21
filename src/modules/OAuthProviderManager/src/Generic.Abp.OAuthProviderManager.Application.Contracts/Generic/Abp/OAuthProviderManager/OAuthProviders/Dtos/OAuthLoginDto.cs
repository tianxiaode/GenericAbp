using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;

[Serializable]
public class OAuthLoginDto
{
    [Required]
    [DisplayName("OAuthProvider:Provider")]
    public string Provider { get; set; }

    [Required] [DisplayName("")] public string ReturnUrl { get; set; }

    [Required] [DisplayName("")] public string ReturnUrlHash { get; set; }
}