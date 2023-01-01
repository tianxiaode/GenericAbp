using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Validation;

namespace Generic.Abp.OpenIddict.Scopes
{
    public class ScopeCreateOrUpdateInput
    {
        [Required]
        [DynamicStringLength(typeof(OpenIddictScopeConsts), nameof(OpenIddictScopeConsts.NameMaxLength))]
        [DisplayName("Scope:Name")]
        public string Name { get; set; }

        [DisplayName("Scope:Description")]
        public string Description { get; set; }

        [DisplayName("Scope:DisplayName")]
        public string DisplayName { get; set; }
        public List<string> Properties { get; set; }
        public List<string> Resources { get; set; }

    }
}
