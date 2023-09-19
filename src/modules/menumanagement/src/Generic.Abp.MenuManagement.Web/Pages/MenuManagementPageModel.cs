using Generic.Abp.MenuManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.MenuManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class MenuManagementPageModel : AbpPageModel
{
    protected MenuManagementPageModel()
    {
        LocalizationResourceType = typeof(MenuManagementResource);
    }
}
