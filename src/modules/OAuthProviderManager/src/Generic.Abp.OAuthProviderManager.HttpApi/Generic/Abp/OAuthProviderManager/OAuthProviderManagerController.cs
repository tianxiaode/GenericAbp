using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.OAuthProviderManager
{
    public abstract class OAuthProviderManagerController : AbpController
    {
        protected OAuthProviderManagerController()
        {
            LocalizationResource = typeof(OAuthProviderManagerResource);
        }
    }
}
