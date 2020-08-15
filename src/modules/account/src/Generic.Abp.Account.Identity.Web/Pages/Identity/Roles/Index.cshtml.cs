using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Generic.Abp.Account.Identity.Web.Pages.Identity.Roles
{
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
}
