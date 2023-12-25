using Generic.Abp.OpenIddict.Applications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Generic.Abp.Metro.UI.OpenIddict.Web.Pages.OpenIddict.Applications
{
    public class CreateModalModel : OpenIddictPageModel
    {
        public CreateModalModel(IApplicationAppService applicationAppService)
        {
            ApplicationConsentType = new List<SelectListItem>();
            ApplicationTypes = new List<SelectListItem>();
            ClientTypes = new List<SelectListItem>();
            ApplicationAppService = applicationAppService;
            Application = new ApplicationVieModel();
        }

        [BindProperty] public ApplicationVieModel Application { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationTypes { get; set; }
        [BindProperty] public List<SelectListItem> ClientTypes { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationConsentType { get; set; }

        protected IApplicationAppService ApplicationAppService { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationTypes = new List<SelectListItem>()
            {
                new(OpenIddictConstants.ApplicationTypes.Web, OpenIddictConstants.ApplicationTypes.Web),
                new(OpenIddictConstants.ApplicationTypes.Native, OpenIddictConstants.ApplicationTypes.Native),
            };

            ClientTypes = new List<SelectListItem>()
            {
                new SelectListItem(OpenIddictConstants.ClientTypes.Public, OpenIddictConstants.ClientTypes.Public),
                new SelectListItem(OpenIddictConstants.ClientTypes.Confidential,
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
            return await Task.FromResult(Page());
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApplicationAppService.CreateAsync(Application);

            return NoContent();
        }

        public class ApplicationVieModel : ApplicationCreateInput
        {
        }
    }
}