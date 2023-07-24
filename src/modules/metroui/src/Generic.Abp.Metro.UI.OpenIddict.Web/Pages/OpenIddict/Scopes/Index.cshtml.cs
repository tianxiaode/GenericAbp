using Generic.Abp.Metro.UI.OpenIddict.Web.Pages;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.OpenIddict.Web.Pages.OpenIddict.Scopes
{
    public class IndexModel : OpenIddictPageModel
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