using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientClaimCreateInput
{
    [Required]
    [DynamicStringLength(typeof(ClientClaimConsts), nameof(ClientClaimConsts.TypeMaxLength))]
    [DisplayName("ClientClaim:Type")]
    public string Type { get; set; }

    [Required]
    [DynamicStringLength(typeof(ClientClaimConsts), nameof(ClientClaimConsts.ValueMaxLength))]
    [DisplayName("ClientClaim:Value")]
    public string Value { get; set; }
}