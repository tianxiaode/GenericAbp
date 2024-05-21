using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.OAuthProviderManager
{
    public abstract class OAuthProviderManagerAppService : ApplicationService
    {
        protected OAuthProviderManagerAppService()
        {
            LocalizationResource = typeof(OAuthProviderManagerResource);
            ObjectMapperContext = typeof(GenericAbpOAuthProviderManagerApplicationModule);
        }
    }
}
