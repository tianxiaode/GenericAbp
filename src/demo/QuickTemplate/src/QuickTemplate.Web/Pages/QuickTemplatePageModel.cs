using QuickTemplate.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace QuickTemplate.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class QuickTemplatePageModel : AbpPageModel
{
    protected QuickTemplatePageModel()
    {
        LocalizationResourceType = typeof(QuickTemplateResource);
    }
}