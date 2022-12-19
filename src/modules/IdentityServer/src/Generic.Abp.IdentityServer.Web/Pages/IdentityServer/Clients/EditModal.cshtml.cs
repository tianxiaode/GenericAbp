using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class EditModalModel : IdentityServerPageModel
    {
        public EditModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            Client = new ClientViewModel();
            ProtocolTypes = new List<SelectListItem> { 
                new SelectListItem("OpenID Connect", "oidc")
            };
        }

        [BindProperty]
        public ClientViewModel Client { get; set; }

        [BindProperty]
        public List<SelectListItem> ProtocolTypes { get; set; }

        protected IClientAppService ClientAppService { get; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var entity = await ClientAppService.GetAsync(id);
            Client = new ClientViewModel()
            {
                Id = id,
                ClientId = entity.ClientId,
                ClientName = entity.ClientName,
                Enabled = entity.Enabled,
                Description = entity.Description,
                ProtocolType = entity.ProtocolType,
                RequireClientSecret = entity.RequireClientSecret,
                RequireRequestObject = entity.RequireRequestObject,
                RequirePkce = entity.RequirePkce,
                AllowPlainTextPkce = entity.AllowPlainTextPkce,
                AllowOfflineAccess = entity.AllowOfflineAccess,
                AllowAccessTokensViaBrowser = entity.AllowAccessTokensViaBrowser,
                FrontChannelLogoutUri = entity.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = entity.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = entity.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = entity.BackChannelLogoutSessionRequired,
                EnableLocalLogin = entity.EnableLocalLogin,
                UserSsoLifetime = entity.UserSsoLifetime,
                IdentityTokenLifetime = entity.IdentityTokenLifetime,
                AllowedIdentityTokenSigningAlgorithms = entity.AllowedIdentityTokenSigningAlgorithms,
                AccessTokenLifetime = entity.AccessTokenLifetime,
                AccessTokenType = entity.AccessTokenType,
                AuthorizationCodeLifetime = entity.AuthorizationCodeLifetime,
                AbsoluteRefreshTokenLifetime = entity.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = entity.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = entity.RefreshTokenUsage,
                RefreshTokenExpiration = entity.RefreshTokenExpiration,
                UpdateAccessTokenClaimsOnRefresh = entity.UpdateAccessTokenClaimsOnRefresh,
                IncludeJwtId = entity.IncludeJwtId,
                AlwaysIncludeUserClaimsInIdToken = entity.AlwaysIncludeUserClaimsInIdToken,
                ClientClaimsPrefix = entity.ClientClaimsPrefix,
                PairWiseSubjectSalt = entity.PairWiseSubjectSalt,
                RequireConsent  = entity.RequireConsent,
                ConsentLifetime = entity.ConsentLifetime,
                AllowRememberConsent = entity.AllowRememberConsent,
                ClientUri = entity.ClientUri,
                LogoUri = entity.LogoUri,
                UserCodeType = entity.UserCodeType,
                DeviceCodeLifetime = entity.DeviceCodeLifetime,
                ConcurrencyStamp = entity.ConcurrencyStamp,
            };

            return Page() ;


        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.UpdateAsync(Client.Id, Client);

            return NoContent();
        }

        public class ClientViewModel : ClientUpdateInput
        {
            public Guid Id { get; set; }
        }
    }
}
