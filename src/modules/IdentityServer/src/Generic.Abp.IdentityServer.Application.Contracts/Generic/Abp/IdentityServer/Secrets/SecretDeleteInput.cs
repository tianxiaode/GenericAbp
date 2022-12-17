using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Secrets;

[Serializable]
public class SecretDeleteInput
{
    [Required]
    [DisplayName("Secrets:Type")]
    public string Type { get; set; }

    [Required]
    [DisplayName("Secrets:Value")]
    public string Value { get; set; }

}