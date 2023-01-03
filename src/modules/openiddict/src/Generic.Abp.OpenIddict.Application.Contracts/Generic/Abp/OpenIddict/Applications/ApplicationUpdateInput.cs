

using Volo.Abp.Domain.Entities;

namespace Generic.Abp.OpenIddict.Applications
{
    public class ApplicationUpdateInput : ApplicationCreateOrUpdateInput, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}
