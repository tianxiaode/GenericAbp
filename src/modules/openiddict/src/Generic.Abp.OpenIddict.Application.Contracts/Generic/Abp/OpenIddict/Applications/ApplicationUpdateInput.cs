using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Applications
{
    [Serializable]
    public class ApplicationUpdateInput : ApplicationCreateOrUpdateInput, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; } = default!;
    }
}