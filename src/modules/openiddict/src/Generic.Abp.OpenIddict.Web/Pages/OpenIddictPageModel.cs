using Generic.Abp.OpenIddict.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.OpenIddict.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class OpenIddictPageModel : AbpPageModel
{
    protected OpenIddictPageModel()
    {
        LocalizationResourceType = typeof(OpenIddictResource);
    }
}