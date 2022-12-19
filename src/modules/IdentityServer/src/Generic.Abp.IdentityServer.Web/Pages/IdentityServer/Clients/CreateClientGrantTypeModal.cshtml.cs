using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientGrantTypeModalModel : IdentityServerPageModel
    {
        public CreateClientGrantTypeModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            GrantType = new GrantTypeViewModel(){};
        }

        [BindProperty]
        public GrantTypeViewModel GrantType { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            GrantType = new GrantTypeViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddGrantTypeAsync(GrantType.ForeignKeyId, new ClientGrantTypeCreateInput
            {
                GrantType = GrantType.GrantType
            });

            return NoContent();
        }

        public class GrantTypeViewModel: ClientGrantTypeCreateInput
        {           
            public GrantTypeViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public GrantTypeViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }

    }
}
