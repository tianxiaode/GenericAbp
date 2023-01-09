using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.PhoneLogin.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class PhoneLoginPageModel : AbpPageModel
{
    protected PhoneLoginPageModel()
    {
        LocalizationResourceType = typeof(PhoneLoginResource);
    }
}
