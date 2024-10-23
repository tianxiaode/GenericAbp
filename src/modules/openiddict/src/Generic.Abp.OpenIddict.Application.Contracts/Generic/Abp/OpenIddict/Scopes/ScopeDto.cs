using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Scopes
{
    [Serializable]
    public class ScopeDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public Dictionary<string, object> Properties { get; set; } = default!;
        public HashSet<string> Resources { get; set; } = [];
        public string ConcurrencyStamp { get; set; } = default!;
    }
}