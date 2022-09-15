using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientPostLogoutRedirectUriCreateInput
{
    [Required]
    [DisplayName("ClientPostLogoutRedirectUri:PostLogoutRedirectUri")]
    [DynamicStringLength(typeof(ClientPostLogoutRedirectUriConsts), nameof(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLength))]
    public string PostLogoutRedirectUri { get; set; }
}