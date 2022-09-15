using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.ApiScopes;

[Serializable]
public class ApiScopeCreateOrUpdateInput
{

    [DisplayName("ApiScope:DisplayName")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.DisplayNameMaxLength) )]
    public string DisplayName { get; set; }

    [DisplayName("ApiScope:Description")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.DescriptionMaxLength) )]
    public string Description { get; set; }

    [DisplayName("ApiScope:Enabled")]
    public bool Enabled { get; set; }

    [DisplayName("ApiScope:Required")]
    public bool Required { get; set; }

    [DisplayName("ApiScope:Emphasize")]
    public bool Emphasize { get; set; }

    [DisplayName("ApiScope:ShowInDiscoveryDocument")]
    public bool ShowInDiscoveryDocument { get; set; }


}