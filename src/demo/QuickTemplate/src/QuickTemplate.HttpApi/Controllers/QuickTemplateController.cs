using QuickTemplate.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace QuickTemplate.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class QuickTemplateController : AbpControllerBase
{
    protected QuickTemplateController()
    {
        LocalizationResource = typeof(QuickTemplateResource);
    }
}
