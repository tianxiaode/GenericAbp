using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.OpenIddict.Scopes
{
    [Serializable]
    public class ScopeCreateOrUpdateInput
    {
        [Required] [DisplayName("Scope:Name")] public string Name { get; set; } = default!;

        [DisplayName("Scope:Description")] public string Description { get; set; } = default!;

        [DisplayName("Scope:DisplayName")] public string DisplayName { get; set; } = default!;

        [DisplayName("Scope:Properties")] public Dictionary<string, object> Properties { get; set; } = default!;

        [DisplayName("Scope:Resources")] public HashSet<string> Resources { get; set; } = default!;
    }
}