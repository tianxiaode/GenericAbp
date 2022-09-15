using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceCreateInput: ApiResourceCreateOrUpdateInput
{
    [Required]
    [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.NameMaxLength))]
    [DisplayName("ApiResource:Name")]
    public string Name { get; set; }

}