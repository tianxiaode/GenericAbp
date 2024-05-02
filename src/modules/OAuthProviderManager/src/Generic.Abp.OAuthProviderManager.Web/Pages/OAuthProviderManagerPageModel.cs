using Generic.Abp.OAuthProviderManager.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.OAuthProviderManager.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class OAuthProviderManagerPageModel : AbpPageModel
{
    protected OAuthProviderManagerPageModel()
    {
        LocalizationResourceType = typeof(OAuthProviderManagerResource);
    }
}
