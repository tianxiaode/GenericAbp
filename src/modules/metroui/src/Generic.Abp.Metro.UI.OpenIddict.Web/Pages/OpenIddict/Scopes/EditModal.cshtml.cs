using Generic.Abp.Metro.UI.OpenIddict.Web.Pages;
using Generic.Abp.OpenIddict.Scopes;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.OpenIddict.Web.Pages.OpenIddict.Scopes
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
            Scope.PropertiesStr = string.Join(',', entity.Properties);
            Scope.ResourcesStr = string.Join(',', entity.Resources);
            return await Task.FromResult(Page());
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            Scope.Properties = Scope.PropertiesStr?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            Scope.Resources = Scope.ResourcesStr?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            await ScopeAppService.UpdateAsync(Scope.Id, Scope);

            return NoContent();
        }

        public class ScopeVieModel : ScopeUpdateInput
        {
            public Guid Id { get; set; }
            public string? PropertiesStr { get; set; }

            public string? ResourcesStr { get; set; }
        }
    }
}