using Generic.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class CreateScopesModalModel : IdentityServerPageModel
    {
        protected IApiResourceAppService ApiResourceAppService { get; }

        public CreateScopesModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
        }

        [BindProperty]
        public ApiResourceScopeViewModal ApiResourceScope { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ApiResourceScope = new ApiResourceScopeViewModal(){
                ApiResourceId = id
            };

            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.AddScopeAsync(ApiResourceScope.ApiResourceId, ApiResourceScope);

            return NoContent();
        }

         public class ApiResourceScopeViewModal: ApiResourceScopeCreateInput
        {
            public Guid ApiResourceId { get; set; }
        }

    }
}
