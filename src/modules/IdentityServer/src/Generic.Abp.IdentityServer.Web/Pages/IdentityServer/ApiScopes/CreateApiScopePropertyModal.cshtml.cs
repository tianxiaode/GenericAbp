using Generic.Abp.IdentityServer.ApiResources;
using Generic.Abp.IdentityServer.ApiScopes;
using Generic.Abp.IdentityServer.Properties;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiScopes
{
    public class CreateApiScopePropertyModalModel : IdentityServerPageModel
    {
        public CreateApiScopePropertyModalModel(IApiScopeAppService apiScopeAppService)
        {
            ApiScopeAppService = apiScopeAppService;
            Property = new PropertyViewModel(){};
        }

        [BindProperty]
        public PropertyViewModel Property { get; set; }

        public IApiScopeAppService ApiScopeAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Property = new PropertyViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiScopeAppService.AddPropertyAsync(Property.ForeignKeyId, new ApiScopePropertyCreateInput
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
