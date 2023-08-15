using Generic.Abp.OpenIddict.Scopes;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.OpenIddict.Web.Pages.OpenIddict.Scopes
{
    public class CreateModalModel : OpenIddictPageModel
    {
        public CreateModalModel(IScopeAppService scopeAppService)
        {
            ScopeAppService = scopeAppService;
            Scope = new ScopeVieModel();
        }

        [BindProperty] public ScopeVieModel Scope { get; set; }

        protected IScopeAppService ScopeAppService { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return await Task.FromResult(Page());
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ScopeAppService.CreateAsync(Scope);

            return NoContent();
        }

        public class ScopeVieModel : ScopeCreateInput
        {
        }
    }
}