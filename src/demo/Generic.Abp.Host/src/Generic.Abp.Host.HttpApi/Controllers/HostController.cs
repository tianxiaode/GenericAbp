using Generic.Abp.Host.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Host.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class HostController : AbpControllerBase
{
    protected HostController()
    {
        LocalizationResource = typeof(HostResource);
    }
}
