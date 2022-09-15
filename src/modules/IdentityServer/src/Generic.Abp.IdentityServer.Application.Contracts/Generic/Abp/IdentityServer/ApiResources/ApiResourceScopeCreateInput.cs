using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceScopeCreateInput
{
    [Required]
    [DisplayName("ApiScope:Name")]
    [DynamicStringLength(typeof(IdentityResourceConsts),nameof(IdentityResourceConsts.NameMaxLength) )]
    public string Name { get; set; }

}