

using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Scopes
{
    public class ScopeUpdateInput : ScopeCreateOrUpdateInput, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}
