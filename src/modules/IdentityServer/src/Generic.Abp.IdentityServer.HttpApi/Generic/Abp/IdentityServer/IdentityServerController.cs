using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.IdentityServer.Localization;

namespace Generic.Abp.IdentityServer
{
    public abstract class IdentityServerController : AbpController
    {
        protected IdentityServerController()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }
    }
}
