using Generic.Abp.IdentityServer.IdentityResources;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.IdentityResources
{
    public class EditModalModel : IdentityServerPageModel
    {
        public EditModalModel(IIdentityResourceAppService identityResourceAppService)
        {
            IdentityResourceAppService = identityResourceAppService;
            IdentityResource = new IdentityResourceViewModel();
        }

        [BindProperty]
        public IdentityResourceViewModel IdentityResource { get; set; }

        protected IIdentityResourceAppService IdentityResourceAppService { get; }
        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            var entity = await IdentityResourceAppService.GetAsync(id);
            IdentityResource = new IdentityResourceViewModel(entity.Id)
            {
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Enabled = entity.Enabled,
                Required = entity.Required,
                Emphasize = entity.Emphasize,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ConcurrencyStamp = entity.ConcurrencyStamp
            };

            return Page();

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await IdentityResourceAppService.UpdateAsync(IdentityResource.Id, new IdentityResourceUpdateInput()
            {
                ConcurrencyStamp = IdentityResource.ConcurrencyStamp,
                Name = IdentityResource.Name,
                Description = IdentityResource.Description,
                DisplayName = IdentityResource.DisplayName,
                Enabled = IdentityResource.Enabled,
                Required = IdentityResource.Required,
                Emphasize = IdentityResource.Emphasize,
                ShowInDiscoveryDocument = IdentityResource.ShowInDiscoveryDocument
            });

            return NoContent();
        }
        public class IdentityResourceViewModel : IdentityResourceUpdateInput
        {
            public IdentityResourceViewModel()
            {
                Id = Guid.NewGuid();
            }

            public IdentityResourceViewModel(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }

        }
    }
}
