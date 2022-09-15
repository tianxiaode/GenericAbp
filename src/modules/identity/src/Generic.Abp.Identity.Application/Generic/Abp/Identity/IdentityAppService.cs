using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Localization;

namespace Generic.Abp.Identity
{
    public abstract class IdentityAppService : ApplicationService
    {
        protected IdentityAppService()
        {
            LocalizationResource = typeof(IdentityResource);
            ObjectMapperContext = typeof(GenericAbpIdentityApplicationModule);
        }
    }
}
