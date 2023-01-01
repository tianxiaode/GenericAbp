using Generic.Abp.OpenIddict.Scopes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Generic.Abp.OpenIddict.Web.Pages.OpenIddict.Scopes
{
    public class CreateModalModel : OpenIddictPageModel
    {
        public CreateModalModel(IScopeAppService scopeAppService)
        {
            ScopeAppService = scopeAppService;
            Scope = new ScopeVieModel();
        }

        [BindProperty]
        public ScopeVieModel Scope { get; set; }

        protected IScopeAppService ScopeAppService { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return await Task.FromResult(Page());

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            Scope.Properties = Scope.PropertiesStr?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Scope.Resources = Scope.ResourcesStr?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            await ScopeAppService.CreateAsync(Scope);

            return NoContent();
        }

        public class ScopeVieModel : ScopeCreateInput
        {

            public string? PropertiesStr { get; set; }

            public string? ResourcesStr { get; set; }
        }
    }
}
