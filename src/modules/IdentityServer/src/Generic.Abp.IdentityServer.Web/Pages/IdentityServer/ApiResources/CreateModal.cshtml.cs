using Generic.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class CreateModalModel : IdentityServerPageModel
    {
        public CreateModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
            ApiResource = new ApResourceViewModel()
            {
                Enabled = true,
                ShowInDiscoveryDocument = true
            };
        }

        [BindProperty]
        public ApResourceViewModel ApiResource { get; set; }

        protected IApiResourceAppService ApiResourceAppService { get; }
        public async Task<IActionResult> OnGetAsync()
        {

            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.CreateAsync(ApiResource);

            return NoContent();
        }

        public class ApResourceViewModel : ApiResourceCreateInput
        {

        }

    }
}
