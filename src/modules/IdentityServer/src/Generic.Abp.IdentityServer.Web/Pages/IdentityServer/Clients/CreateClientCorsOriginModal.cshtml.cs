using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientCorsOriginModalModel : IdentityServerPageModel
    {
        public CreateClientCorsOriginModalModel(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
            CorsOrigin = new CorsOriginViewModel(){};
        }

        [BindProperty]
        public CorsOriginViewModel CorsOrigin { get; set; }

        public IClientAppService ClientAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            CorsOrigin = new CorsOriginViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddCorsOriginAsync(CorsOrigin.ForeignKeyId, new ClientCorsOriginCreateInput
            {
                Origin = CorsOrigin.Origin
            });

            return NoContent();
        }

        public class CorsOriginViewModel: ClientCorsOriginCreateInput
        {           
            public CorsOriginViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public CorsOriginViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }


    }
}
