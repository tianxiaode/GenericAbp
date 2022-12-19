using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateModalModel : IdentityServerPageModel
    {
        public CreateModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            Client = new ClientViewModel()
            {
                Enabled = true,
                RequireClientSecret = true,
                AllowRememberConsent = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = true,
                FrontChannelLogoutSessionRequired = true,
                BackChannelLogoutSessionRequired = true,
                AllowOfflineAccess = true,
                IdentityTokenLifetime= 300,
                AuthorizationCodeLifetime = 300,
                AccessTokenLifetime = 31536000,
                EnableLocalLogin = true,
                DeviceCodeLifetime = 300,
                AccessTokenType = 0,
                ProtocolType = "oidc"
            };
            ProtocolTypes = new List<SelectListItem> { 
                new SelectListItem("OpenID Connect","oidc")
            };
        }

        [BindProperty]
        public ClientViewModel Client { get; set; }

        [BindProperty]
        public List<SelectListItem> ProtocolTypes { get; set; }

        protected IClientAppService ClientAppService { get; }
        public async Task<IActionResult> OnGetAsync()
        {
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.CreateAsync(Client);

            return NoContent();
        }

        public class ClientViewModel : ClientCreateInput
        {

        }
    }
}
