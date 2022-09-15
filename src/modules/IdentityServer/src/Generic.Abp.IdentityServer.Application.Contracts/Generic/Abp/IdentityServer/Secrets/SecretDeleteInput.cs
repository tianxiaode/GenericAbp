using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.Enumeration.Validation;
using Generic.Abp.IdentityServer.Enumerations;

namespace Generic.Abp.IdentityServer.Secrets;

[Serializable]
public class SecretDeleteInput
{
    [Required]
    [DisplayName("Secrets:Type")]
    [EnumValue(typeof(SecretType))]
    public string Type { get; set; }

    [Required]
    [DisplayName("Secrets:Value")]
    public string Value { get; set; }

}