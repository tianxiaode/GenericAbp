using Volo.Abp.Application.Services;
using Volo.Abp.IdentityServer.Localization;

namespace Generic.Abp.IdentityServer
{
    public abstract class IdentityServerAppService : ApplicationService
    {
        protected IdentityServerAppService()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
            ObjectMapperContext = typeof(GenericAbpIdentityServerApplicationModule);
        }
    }
}
