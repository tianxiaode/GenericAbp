using Generic.Abp.Metro.UI.OpenIddict.Web.Pages;
using Generic.Abp.OpenIddict.Applications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenIddict.Abstractions;

namespace Generic.Abp.OpenIddict.Web.Pages.OpenIddict.Applications
{
    public class EditModalModel : OpenIddictPageModel
    {
        public EditModalModel(IApplicationAppService applicationAppService)
        {
            ApplicationTypes = new List<SelectListItem>();
            ApplicationConsentType = new List<SelectListItem>();
            ApplicationAppService = applicationAppService;
            Application = new ApplicationVieModel();
        }

        [BindProperty] public ApplicationVieModel Application { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationTypes { get; set; }

        [BindProperty] public List<SelectListItem> ApplicationConsentType { get; set; }

        protected IApplicationAppService ApplicationAppService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ApplicationTypes = new List<SelectListItem>()
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

            var entity = await ApplicationAppService.GetAsync(id);
            Application = new ApplicationVieModel()
            {
                Id = id,
                ClientId = entity.ClientId,
                DisplayName = entity.DisplayName,
                Type = entity.Type,
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