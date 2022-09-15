using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.IdentityResources;

[Serializable]
public class IdentityResourceCreateOrUpdateInput
{
    [Required]
    [DisplayName("IdentityResource:Name")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.NameMaxLength) )]
    public virtual string Name { get; set; }

    [DisplayName("IdentityResource:DisplayName")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.DisplayNameMaxLength) )]
    public virtual string DisplayName { get; set; }

    [DisplayName("IdentityResource:Description")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.DescriptionMaxLength) )]
    public virtual string Description { get; set; }

    [DisplayName("IdentityResource:Enabled")]
    public virtual bool Enabled { get; set; }

    [DisplayName("IdentityResource:Required")]
    public virtual bool Required { get; set; }

    [DisplayName("IdentityResource:Emphasize")]
    public virtual bool Emphasize { get; set; }

    [DisplayName("IdentityResource:ShowInDiscoveryDocument")]
    public virtual bool ShowInDiscoveryDocument { get; set; }


}