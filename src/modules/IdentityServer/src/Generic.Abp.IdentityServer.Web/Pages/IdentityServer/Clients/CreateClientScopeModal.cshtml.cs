using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientScopeModalModel : IdentityServerPageModel
    {
        public CreateClientScopeModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            Scope = new ScopeViewModel(){};
        }

        [BindProperty]
        public ScopeViewModel Scope { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Scope = new ScopeViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddScopeAsync(Scope.ForeignKeyId, new ClientScopeCreateInput
            {
                Scope = Scope.Scope
            });

            return NoContent();
        }

        public class ScopeViewModel: ClientScopeCreateInput
        {           
            public ScopeViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public ScopeViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }

    }
}
