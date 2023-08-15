using Generic.Abp.OpenIddict.Scopes;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.OpenIddict.Web.Pages.OpenIddict.Scopes
{
    public class EditModalModel : OpenIddictPageModel
    {
        public EditModalModel(IScopeAppService scopeAppService)
        {
            ScopeAppService = scopeAppService;
            Scope = new ScopeVieModel();
        }

        [BindProperty] public ScopeVieModel Scope { get; set; }

        protected IScopeAppService ScopeAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var entity = await ScopeAppService.GetAsync(id);
            Scope.Id = entity.Id;
            Scope.ConcurrencyStamp = entity.ConcurrencyStamp;
            Scope.Name = entity.Name;
            Scope.Description = entity.Description;
            Scope.DisplayName = entity.DisplayName;
            Scope.Properties = entity.Properties;
            Scope.Resources = entity.Resources;
            return await Task.FromResult(Page());
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();


            await ScopeAppService.UpdateAsync(Scope.Id, Scope);

            return NoContent();
        }

        public class ScopeVieModel : ScopeUpdateInput
        {
            public Guid Id { get; set; }
        }
    }
}