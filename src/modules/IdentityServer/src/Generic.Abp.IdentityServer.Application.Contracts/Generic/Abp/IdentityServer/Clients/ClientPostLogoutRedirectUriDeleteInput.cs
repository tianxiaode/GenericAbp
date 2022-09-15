using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientPostLogoutRedirectUriDeleteInput
{
    [Required]
    [DisplayName("ClientPostLogoutRedirectUri:PostLogoutRedirectUri")]
    public string PostLogoutRedirectUri { get; set; }

}