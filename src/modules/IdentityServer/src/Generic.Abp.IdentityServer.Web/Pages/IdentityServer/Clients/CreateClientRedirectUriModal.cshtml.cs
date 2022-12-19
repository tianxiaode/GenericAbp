using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientRedirectUriModalModel : IdentityServerPageModel
    {
        public CreateClientRedirectUriModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            RedirectUri = new RedirectUriViewModel(){};
        }

        [BindProperty]
        public RedirectUriViewModel RedirectUri { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            RedirectUri = new RedirectUriViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddRedirectUriAsync(RedirectUri.ForeignKeyId, new ClientRedirectUriCreateInput
            {
                RedirectUri = RedirectUri.RedirectUri
            });

            return NoContent();
        }

        public class RedirectUriViewModel: ClientRedirectUriCreateInput
        {           
            public RedirectUriViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public RedirectUriViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }

    }
}
