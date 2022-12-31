using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.MyProjectName.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class MyProjectNamePageModel : AbpPageModel
{
    protected MyProjectNamePageModel()
    {
        LocalizationResourceType = typeof(MyProjectNameResource);
    }
}
