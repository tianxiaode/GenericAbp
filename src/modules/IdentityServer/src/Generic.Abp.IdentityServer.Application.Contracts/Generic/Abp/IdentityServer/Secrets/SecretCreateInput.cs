using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Secrets;

public class SecretCreateInput
{
    [Required]
    [DisplayName("Secrets:Type")]
    [DynamicStringLength(typeof(ClientSecretConsts), nameof(ClientSecretConsts.TypeMaxLength))]
    public string Type { get; set; }

    [Required]
    [DisplayName("Secrets:Value")]
    [DynamicStringLength(typeof(ClientSecretConsts), nameof(ClientSecretConsts.ValueMaxLength))]
    public string Value { get; set; }

    [DisplayName("Secrets:Description")]
    [DynamicStringLength(typeof(ClientSecretConsts), nameof(ClientSecretConsts.DescriptionMaxLength))]
    public string Description { get; set; }

    [DisplayName("Secrets:Expiration")]
    public DateTime? Expiration { get; set; }

}