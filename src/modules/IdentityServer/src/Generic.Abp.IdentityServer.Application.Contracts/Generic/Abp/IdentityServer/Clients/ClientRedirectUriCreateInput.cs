using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientRedirectUriCreateInput
{
    [Required]
    [DisplayName("ClientRedirectUri:RedirectUri")]
    [DynamicMaxLength(typeof(ClientRedirectUriConsts),nameof(ClientRedirectUriConsts.RedirectUriMaxLength))]
    public string RedirectUri { get; set; }
}