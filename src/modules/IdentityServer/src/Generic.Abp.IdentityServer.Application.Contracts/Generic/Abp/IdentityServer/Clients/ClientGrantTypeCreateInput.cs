using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientGrantTypeCreateInput
{
    [Required]
    [DynamicStringLength(typeof(ClientGrantTypeConsts),nameof(ClientGrantTypeConsts.GrantTypeMaxLength))]
    [DisplayName("ClientGrantType:GrantType")]
    public string GrantType { get; set; }
}