using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.Metro.UI.OpenIddict.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class OpenIddictPageModel : AbpPageModel
{
    protected OpenIddictPageModel()
    {
        LocalizationResourceType = typeof(OpenIddictResource);
    }
}