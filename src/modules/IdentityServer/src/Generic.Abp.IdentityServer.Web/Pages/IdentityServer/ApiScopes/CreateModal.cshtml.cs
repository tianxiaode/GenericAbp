using Generic.Abp.IdentityServer.ApiScopes;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiScopes
{
    public class CreateModalModel : IdentityServerPageModel
    {
        public CreateModalModel(IApiScopeAppService apiScopeAppService)
        {
            ApiScopeAppService = apiScopeAppService;
            ApiScope = new ApScopeViewModel();
        }

        [BindProperty]
        public ApScopeViewModel ApiScope { get; set; }

        protected IApiScopeAppService ApiScopeAppService { get; }
        public async Task<IActionResult> OnGetAsync()
        {
            ApiScope = new ApScopeViewModel()
            {

                Enabled = true,
                ShowInDiscoveryDocument = true,
                Emphasize=false,
                Required = false,
            };

            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiScopeAppService.CreateAsync(ApiScope);

            return NoContent();
        }

        public class ApScopeViewModel : ApiScopeCreateInput
        {

        }

    }
}
