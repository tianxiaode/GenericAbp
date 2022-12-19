using Generic.Abp.IdentityServer.Clients;
using Generic.Abp.IdentityServer.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientSecretModalModel : IdentityServerPageModel
    {
        public CreateClientSecretModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            Secret = new ClientSecretViewModel(){};
            SecretTypes = new List<SelectListItem>()
            {
                new SelectListItem("Shared Secret" ,IdentityServerConstants.SecretTypes.SharedSecret ),
                new SelectListItem("X509 Thumbprint",IdentityServerConstants.SecretTypes.X509CertificateThumbprint)
            };
        }

        [BindProperty]
        public ClientSecretViewModel Secret { get; set; }

        [BindProperty]
        public List<SelectListItem > SecretTypes { get; set; }

        protected IClientAppService ClientAppService { get; }


        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Secret = new ClientSecretViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddSecretAsync(Secret.ForeignKeyId, new ClientSecretCreateInput
            {
                Type = Secret.Type,
                Description = Secret.Description,
                Value = Secret.Value,
                Expiration = Secret.Expiration
            });

            return NoContent();
        }

        public class ClientSecretViewModel: SecretCreateInput
        {
            public ClientSecretViewModel()
            {
                Type = IdentityServerConstants.SecretTypes.SharedSecret;
                ForeignKeyId = Guid.NewGuid();
                Value = "";
                Description ="";
            }

            public ClientSecretViewModel(Guid foreignKeyId)
            {
                Type = IdentityServerConstants.SecretTypes.SharedSecret;
                ForeignKeyId = foreignKeyId;
                Value = "";
                Description ="";
            }

            public Guid ForeignKeyId { get; set; }



        }
    }
}
