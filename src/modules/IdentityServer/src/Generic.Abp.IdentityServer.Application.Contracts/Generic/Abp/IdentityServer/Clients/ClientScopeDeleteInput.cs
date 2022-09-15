using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientScopeDeleteInput
{
    [Required]
    [DisplayName("ClientScope:Scope")]
    public string Scope { get; set; }

}