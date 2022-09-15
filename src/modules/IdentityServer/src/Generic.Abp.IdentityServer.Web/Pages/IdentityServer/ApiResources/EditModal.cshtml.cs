using Generic.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class EditModalModel : IdentityServerPageModel
    {
        public EditModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
        }

        [BindProperty]
        public ApResourceViewModel ApiResource { get; set; }

        protected IApiResourceAppService ApiResourceAppService { get; }
        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            var entity = await ApiResourceAppService.GetAsync(id);
            ApiResource = new ApResourceViewModel(entity.Id, entity.Name)
            {
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Enabled = entity.Enabled,
                AllowedAccessTokenSigningAlgorithms = entity.AllowedAccessTokenSigningAlgorithms,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ConcurrencyStamp = entity.ConcurrencyStamp
            };

            return Page() ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.UpdateAsync(ApiResource.Id, new ApiResourceUpdateInput()
            {
                AllowedAccessTokenSigningAlgorithms = ApiResource.AllowedAccessTokenSigningAlgorithms,
                ConcurrencyStamp = ApiResource.ConcurrencyStamp,
                Description = ApiResource.Description,
                DisplayName = ApiResource.DisplayName,
                Enabled = ApiResource.Enabled,
                ShowInDiscoveryDocument = ApiResource.ShowInDiscoveryDocument
            });

            return NoContent();
        }
        public class ApResourceViewModel : ApiResourceUpdateInput
        {
            public ApResourceViewModel(Guid id, string name)
            {
                Id = id;
                Name = name;
            }

            public ApResourceViewModel()
            {
            }

            public Guid Id { get; set; }

            [ReadOnlyInput]
            public string Name { get; set; }
        }
    }


}
