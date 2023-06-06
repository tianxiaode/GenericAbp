using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.Metro.UI.Identity.Web.Pages.Identity;

public abstract class IdentityPageModel : AbpPageModel
{
    protected IdentityPageModel()
    {
        ObjectMapperContext = typeof(AbpIdentityWebModule);
    }
}