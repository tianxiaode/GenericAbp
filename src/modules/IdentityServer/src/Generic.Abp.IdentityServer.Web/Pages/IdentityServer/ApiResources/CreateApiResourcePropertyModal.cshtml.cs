using Generic.Abp.IdentityServer.ApiResources;
using Generic.Abp.IdentityServer.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources.CreateApiResourceSecretModalModel;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class CreateApiResourcePropertyModalModel : IdentityServerPageModel
    {
        public CreateApiResourcePropertyModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
            Property = new PropertyViewModel(){};
        }

        [BindProperty]
        public PropertyViewModel Property { get; set; }

        public IApiResourceAppService ApiResourceAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Property = new PropertyViewModel(foreignKeyId);
            Property.ForeignKeyId = foreignKeyId;
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.AddPropertyAsync(Property.ForeignKeyId, new ApiResourcePropertyCreateInput
            {
                Key = Property.Key,
                Value = Property.Value,
            });

            return NoContent();

        }

        public class PropertyViewModel: PropertyCreateInput
        {
            public PropertyViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
                Key = "";
                Value = "";
            }

            public PropertyViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
                Key = "";
                Value = "";
            }

            public Guid ForeignKeyId { get; set; }
        }
    }
}
