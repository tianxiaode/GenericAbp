using Generic.Abp.IdentityServer.ClaimTypes;
using Generic.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.Clients
{
    public class CreateClientClaimModalModel : IdentityServerPageModel
    {
        public CreateClientClaimModalModel(IClientAppService clientAppService, IClaimTypeAppService claimTypeAppService)
        {
            ClientAppService = clientAppService;
            ClaimTypeAppService = claimTypeAppService;
            ClaimTypes = new List<SelectListItem>();
            Claim = new ClaimViewModel() { };
        }

        [BindProperty]
        public ClaimViewModel Claim { get; set; }

        [BindProperty]
        public List<SelectListItem> ClaimTypes { get; set; }
        public IClientAppService ClientAppService { get; set; }
        public IClaimTypeAppService ClaimTypeAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            Claim = new ClaimViewModel(foreignKeyId);
            ClaimTypes = (await ClaimTypeAppService.GetListAsync()).Items.Select(m => new SelectListItem(m.Name, m.Name)).ToList();
            return await Task.FromResult(Page());

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ClientAppService.AddClaimAsync(Claim.ForeignKeyId, new ClientClaimCreateInput
            {
                Type = Claim.Type,
                Value = Claim.Value
            });

            return NoContent();
        }

        public class ClaimViewModel : ClientClaimCreateInput
        {
            public ClaimViewModel()
            {
                ForeignKeyId = Guid.NewGuid();
            }

            public ClaimViewModel(Guid foreignKeyId)
            {
                ForeignKeyId = foreignKeyId;
            }

            public Guid ForeignKeyId { get; set; }
        }
    }
}
