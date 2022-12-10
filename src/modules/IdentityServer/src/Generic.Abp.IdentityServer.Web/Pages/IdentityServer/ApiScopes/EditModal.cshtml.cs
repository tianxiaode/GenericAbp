using Generic.Abp.IdentityServer.ApiScopes;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiScopes
{
    public class EditModalModel : IdentityServerPageModel
    {
        public EditModalModel(IApiScopeAppService apiScopeAppService)
        {
            ApiScopeAppService = apiScopeAppService;
            ApiScope = new ApiScopeViewModel();
        }

        [BindProperty]
        public ApiScopeViewModel ApiScope { get; set; }

        protected IApiScopeAppService ApiScopeAppService { get; }
        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            var entity = await ApiScopeAppService.GetAsync(id);
            ApiScope = new ApiScopeViewModel(entity.Id, entity.Name)
            {
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Enabled = entity.Enabled,
                Emphasize = entity.Emphasize,
                Required = entity.Required,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ConcurrencyStamp = entity.ConcurrencyStamp
            };

            return Page() ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiScopeAppService.UpdateAsync(ApiScope.Id, new ApiScopeUpdateInput()
            {
                ConcurrencyStamp = ApiScope.ConcurrencyStamp,
                Description = ApiScope.Description,
                DisplayName = ApiScope.DisplayName,
                Enabled = ApiScope.Enabled,
                ShowInDiscoveryDocument = ApiScope.ShowInDiscoveryDocument,
                Required = ApiScope.Required,
                Emphasize = ApiScope.Emphasize,
            });

            return NoContent();
        }
        public class ApiScopeViewModel : ApiScopeUpdateInput
        {
            public ApiScopeViewModel()
            {
                Id = Guid.NewGuid();
                Name = "";
            }

            public ApiScopeViewModel(Guid id, string name)
            {
                Id = id;
                Name = name;
            }

            public Guid Id { get; set; }

            [ReadOnlyInput]
            public string Name { get; set; }
        }
    }


}
