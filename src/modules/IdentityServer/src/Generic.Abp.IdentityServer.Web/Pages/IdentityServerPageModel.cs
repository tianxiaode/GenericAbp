using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.IdentityServer.Localization;

namespace Generic.Abp.IdentityServer.Web.Pages;

public abstract class IdentityServerPageModel : AbpPageModel
{
    protected IdentityServerPageModel()
    {
        LocalizationResourceType = typeof(AbpIdentityServerResource);
        ObjectMapperContext = typeof(GenericAbpIdentityServerWebAutoMapperProfile);
    }

}