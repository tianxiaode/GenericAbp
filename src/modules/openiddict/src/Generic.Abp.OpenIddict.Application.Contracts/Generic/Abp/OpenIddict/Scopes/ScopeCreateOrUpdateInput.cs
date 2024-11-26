using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Generic.Abp.OpenIddict.Scopes
{
    [Serializable]
    public class ScopeCreateOrUpdateInput : ExtensibleObject
    {
        [Required] [DisplayName("Scope:Name")] public string Name { get; set; } = default!;

        [DisplayName("Scope:Description")] public string? Description { get; set; } = default!;

        [DisplayName("Scope:DisplayName")] public string? DisplayName { get; set; } = default!;

        // [DisplayName("Scope:Properties")] public Dictionary<string, object> Properties { get; set; } = default!;

        [DisplayName("Scope:Resources")] public HashSet<string> Resources { get; set; } = default!;
    }
}