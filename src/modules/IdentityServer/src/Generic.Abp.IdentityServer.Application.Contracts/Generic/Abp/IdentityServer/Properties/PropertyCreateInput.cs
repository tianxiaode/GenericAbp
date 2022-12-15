using System.ComponentModel;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Properties
{
    public class PropertyCreateInput
    {
    [DynamicStringLength(typeof(ApiResourcePropertyConsts), nameof(ApiResourcePropertyConsts.KeyMaxLength))]
    [DisplayName("Properties:Key")]
    public string Key { get; set; }

    [DynamicStringLength(typeof(ApiResourcePropertyConsts), nameof(ApiResourcePropertyConsts.ValueMaxLength))]
    [DisplayName("Properties:Value")]
    public string Value { get; set; }


    }
}
