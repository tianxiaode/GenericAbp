using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.Account.Identity.Web.Pages.Identity
{
    public abstract class IdentityPageModel : AbpPageModel
    {
        protected IdentityPageModel()
        {
            ObjectMapperContext = typeof(GenericAbpIdentityWebModule);
        }
    }
}
