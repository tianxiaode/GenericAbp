using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Identity.Web.Pages.Identity.Roles;

public class IndexModel : IdentityPageModel
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