using Generic.Abp.ExportManager.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.ExportManager.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ExportManagerPageModel : AbpPageModel
{
    protected ExportManagerPageModel()
    {
        LocalizationResourceType = typeof(ExportManagerResource);
    }
}
