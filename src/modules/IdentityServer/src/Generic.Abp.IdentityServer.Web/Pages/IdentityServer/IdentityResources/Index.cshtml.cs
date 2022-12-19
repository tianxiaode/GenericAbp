using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.IdentityResources
{
    public class IndexModel : IdentityServerPageModel
    {
    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
    }
}
