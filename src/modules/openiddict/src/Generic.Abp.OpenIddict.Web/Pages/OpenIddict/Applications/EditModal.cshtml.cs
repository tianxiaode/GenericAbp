using Generic.Abp.OpenIddict.Applications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Generic.Abp.OpenIddict.Web.Pages.OpenIddict.Applications
{
    public class EditModalModel : OpenIddictPageModel
    {
        public EditModalModel(IApplicationAppService applicationAppService)
        {
            ClientTypes = new List<SelectListItem>();
            ApplicationConsentType = new List<SelectListItem>();
            ApplicationTypes = new List<SelectListItem>();
            ApplicationAppService = applicationAppService;
            Application = new ApplicationVieModel();
        }

        [BindProperty] public ApplicationVieModel Application { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationTypes { get; set; }

        [BindProperty] public List<SelectListItem> ClientTypes { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationConsentType { get; set; }

        protected IApplicationAppService ApplicationAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ApplicationTypes = new List<SelectListItem>()
            {
                new(OpenIddictConstants.ApplicationTypes.Web, OpenIddictConstants.ApplicationTypes.Web),
                new(OpenIddictConstants.ApplicationTypes.Native, OpenIddictConstants.ApplicationTypes.Native),
            };

            ClientTypes = new List<SelectListItem>
            {
                new(OpenIddictConstants.ClientTypes.Public, OpenIddictConstants.ClientTypes.Public),
                new(OpenIddictConstants.ClientTypes.Confidential,
                    OpenIddictConstants.ClientTypes.Confidential),
            };
            ApplicationConsentType = new List<SelectListItem>()
            {
                new SelectListItem(OpenIddictConstants.ConsentTypes.Implicit,
                    OpenIddictConstants.ConsentTypes.Implicit),
                new SelectListItem(OpenIddictConstants.ConsentTypes.Explicit,
                    OpenIddictConstants.ConsentTypes.Explicit),
                new SelectListItem(OpenIddictConstants.ConsentTypes.Systematic,
                    OpenIddictConstants.ConsentTypes.Systematic),
                new SelectListItem(OpenIddictConstants.ConsentTypes.External,
                    OpenIddictConstants.ConsentTypes.External),
            };

            var entity = await ApplicationAppService.GetAsync(id);
            Application = new ApplicationVieModel()
            {
                Id = id,
                ClientId = entity.ClientId,
                DisplayName = entity.DisplayName,
                ApplicationType = entity.ApplicationType,
                ClientType = entity.ClientType,
                ConsentType = entity.ConsentType,
                ConcurrencyStamp = entity.ConcurrencyStamp,
                ClientSecret = entity.ClientSecret,
                ClientUri = entity.ClientUri,
                LogoUri = entity.LogoUri,
            };
            return await Task.FromResult(Page());
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApplicationAppService.UpdateAsync(Application.Id, Application);

            return NoContent();
        }

        public class ApplicationVieModel : ApplicationUpdateInput
        {
            public Guid Id { get; set; }
        }
    }
}