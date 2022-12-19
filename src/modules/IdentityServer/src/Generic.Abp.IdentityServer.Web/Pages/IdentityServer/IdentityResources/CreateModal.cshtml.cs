using Generic.Abp.IdentityServer.IdentityResources;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.IdentityResources
{
    public class CreateModalModel : IdentityServerPageModel
    {
        public CreateModalModel(IIdentityResourceAppService identityResourceAppService)
        {
            IdentityResourceAppService = identityResourceAppService;
            IdentityResource = new IdentityResourceViewModel()
            {
                Enabled = true,
                Emphasize = true,
                ShowInDiscoveryDocument = true
            };
        }

        [BindProperty]
        public IdentityResourceViewModel IdentityResource { get; set; }

        protected IIdentityResourceAppService IdentityResourceAppService { get; }
        public async Task<IActionResult> OnGetAsync()
        {

            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await IdentityResourceAppService.CreateAsync(IdentityResource);

            return NoContent();
        }

        public class IdentityResourceViewModel : IdentityResourceCreateInput
        {

        }
    }
}
