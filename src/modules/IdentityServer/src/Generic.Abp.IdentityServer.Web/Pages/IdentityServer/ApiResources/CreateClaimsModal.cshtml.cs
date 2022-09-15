using Generic.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{


    public class CreateClaimsModalModel : IdentityServerPageModel
    {
        protected IApiResourceAppService ApiResourceAppService { get; }

        public CreateClaimsModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
        }

        [BindProperty]
        public ApiResourceClaimViewModal ApiResourceClaim { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ApiResourceClaim = new ApiResourceClaimViewModal(){
                ApiResourceId = id
            };

            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.AddClaimAsync(ApiResourceClaim.ApiResourceId, ApiResourceClaim);

            return NoContent();
        }

        public class ApiResourceClaimViewModal: ApiResourceClaimCreateInput
        {
            public Guid ApiResourceId { get; set; }
        }

    }

}
