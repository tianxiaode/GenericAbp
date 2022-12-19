using Generic.Abp.IdentityServer.IdentityResources;
using Generic.Abp.IdentityServer.Properties;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.IdentityResources
{
    public class CreateIdentityResourcePropertyModalModel : IdentityServerPageModel
    {
        public CreateIdentityResourcePropertyModalModel(IIdentityResourceAppService identityResourceAppService)
        {
            IdentityResourceAppService = identityResourceAppService;
            Property = new PropertyViewModel() { };
        }

        [BindProperty]
        public PropertyViewModel Property { get; set; }

        public IIdentityResourceAppService IdentityResourceAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Property = new PropertyViewModel(foreignKeyId);
            Property.ForeignKeyId = foreignKeyId;
            return await Task.FromResult(Page());

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await IdentityResourceAppService.AddPropertyAsync(Property.ForeignKeyId, new IdentityResourcePropertyCreateInput
            {
                Key = Property.Key,
                Value = Property.Value,
            });

            return NoContent();
        }

        public class PropertyViewModel : PropertyCreateInput
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
