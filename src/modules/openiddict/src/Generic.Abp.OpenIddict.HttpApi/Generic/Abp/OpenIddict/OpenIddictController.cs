using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.OpenIddict
{
    public abstract class OpenIddictController : AbpController
    {
        protected OpenIddictController()
        {
            LocalizationResource = typeof(OpenIddictResource);
        }
    }
}
