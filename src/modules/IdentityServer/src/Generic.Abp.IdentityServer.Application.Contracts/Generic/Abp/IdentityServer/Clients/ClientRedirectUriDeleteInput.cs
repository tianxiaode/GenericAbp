using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientRedirectUriDeleteInput
{
    [Required]
    [DisplayName("ClientRedirectUri:RedirectUri")]
    public string RedirectUri { get; set; }

}