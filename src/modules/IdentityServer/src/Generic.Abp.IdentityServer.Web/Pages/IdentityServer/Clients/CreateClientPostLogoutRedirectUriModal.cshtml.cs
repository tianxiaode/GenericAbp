using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientPostLogoutRedirectUriModalModel : IdentityServerPageModel
    {
        public CreateClientPostLogoutRedirectUriModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            PostLogoutRedirectUri = new PostLogoutRedirectUriViewModel(){};
        }

        [BindProperty]
        public PostLogoutRedirectUriViewModel PostLogoutRedirectUri { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            PostLogoutRedirectUri = new PostLogoutRedirectUriViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddPostLogoutRedirectUriAsync(PostLogoutRedirectUri.ForeignKeyId, new ClientPostLogoutRedirectUriCreateInput
            {
                PostLogoutRedirectUri = PostLogoutRedirectUri.PostLogoutRedirectUri
            });

            return NoContent();
        }

        public class PostLogoutRedirectUriViewModel: ClientPostLogoutRedirectUriCreateInput
        {           
            public PostLogoutRedirectUriViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public PostLogoutRedirectUriViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }

    }
}
