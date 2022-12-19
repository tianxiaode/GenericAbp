using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientPropertyModalModel : IdentityServerPageModel
    {
        public CreateClientPropertyModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            Property = new PropertyViewModel(){};
        }

        [BindProperty]
        public PropertyViewModel Property { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Property = new PropertyViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddPropertyAsync(Property.ForeignKeyId, new ClientPropertyCreateInput
            {
                Key = Property.Key,
                Value = Property.Value
            });

            return NoContent();
        }

        public class PropertyViewModel: ClientPropertyCreateInput
        {           
            public PropertyViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public PropertyViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }

    }
}
