using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientClaimDeleteInput
{
    [Required]
    [DisplayName("ClientClaim:Type")]
    public string Type { get; set; }

    [Required]
    [DisplayName("ClientClaim:Value")]
    public string Value { get; set; }

}