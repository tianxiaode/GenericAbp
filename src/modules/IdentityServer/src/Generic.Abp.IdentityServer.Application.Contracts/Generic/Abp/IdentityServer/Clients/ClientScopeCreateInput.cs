using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientScopeCreateInput
{
    [Required]
    [DynamicStringLength(typeof(ClientScopeConsts),nameof(ClientScopeConsts.ScopeMaxLength))]
    [DisplayName("ClientScope:Scope")]
    public string Scope { get; set; }
}