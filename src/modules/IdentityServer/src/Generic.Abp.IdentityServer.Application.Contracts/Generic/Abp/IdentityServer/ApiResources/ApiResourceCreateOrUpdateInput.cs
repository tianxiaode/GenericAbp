using System;
using System.ComponentModel;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceCreateOrUpdateInput
{

    [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DisplayNameMaxLength))]
    [DisplayName("ApiResource:DisplayName")]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DescriptionMaxLength))]
    [DisplayName("ApiResource:Description")]
    public string Description { get; set; }

    [DisplayName("ApiResource:Enabled")]
    public bool Enabled { get; set; }

    [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.AllowedAccessTokenSigningAlgorithmsMaxLength))]
    [DisplayName("ApiResource:AllowedAccessTokenSigningAlgorithms")]
    public string AllowedAccessTokenSigningAlgorithms { get; set; }

    [DisplayName("ShowInDiscoveryDocument:Name")]
    public bool ShowInDiscoveryDocument { get; set; } = true;

}