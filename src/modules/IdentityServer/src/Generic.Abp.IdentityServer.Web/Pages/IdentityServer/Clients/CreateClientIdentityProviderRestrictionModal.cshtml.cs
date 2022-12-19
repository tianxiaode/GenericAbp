using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientIdentityProviderRestrictionModalModel : IdentityServerPageModel
    {
        public CreateClientIdentityProviderRestrictionModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            IdentityProviderRestriction = new IdentityProviderRestrictionViewModel(){};
        }

        [BindProperty]
        public IdentityProviderRestrictionViewModel IdentityProviderRestriction { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            IdentityProviderRestriction = new IdentityProviderRestrictionViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddIdentityProviderRestrictionAsync(IdentityProviderRestriction.ForeignKeyId, new ClientIdentityProviderRestrictionCreateInput
            {
                Provider = IdentityProviderRestriction.Provider
            });

            return NoContent();
        }

        public class IdentityProviderRestrictionViewModel: ClientIdentityProviderRestrictionCreateInput
        {           
            public IdentityProviderRestrictionViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public IdentityProviderRestrictionViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }
    }
}
