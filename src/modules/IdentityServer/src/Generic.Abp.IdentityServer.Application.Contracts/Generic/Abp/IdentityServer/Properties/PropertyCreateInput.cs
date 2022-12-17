using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Properties
{
    public class PropertyCreateInput
    {
        [Required]
        [DynamicStringLength(typeof(ApiResourcePropertyConsts), nameof(ApiResourcePropertyConsts.KeyMaxLength))]
        [DisplayName("Properties:Key")]
        public string Key { get; set; }

        [Required]
        [DynamicStringLength(typeof(ApiResourcePropertyConsts), nameof(ApiResourcePropertyConsts.ValueMaxLength))]
        [DisplayName("Properties:Value")]
        public string Value { get; set; }


    }
}
