using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.OpenIddict
{
    public abstract class OpenIddictAppService : ApplicationService
    {
        protected OpenIddictAppService()
        {
            LocalizationResource = typeof(OpenIddictResource);
            ObjectMapperContext = typeof(GenericAbpOpenIddictApplicationModule);
        }
    }
}
