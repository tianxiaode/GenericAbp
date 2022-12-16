using Generic.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using static Generic.Abp.IdentityServer.IdentityServerConstants;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class CreateApiResourceSecretModalModel : IdentityServerPageModel
    {

        public CreateApiResourceSecretModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
            ApiResourceSecret = new ApiResourceSecretViewModel(){};
            SecretTypes = new List<SelectListItem >();
        }

        [BindProperty]
        public ApiResourceSecretViewModel ApiResourceSecret { get; set; }

        [BindProperty]
        public List<SelectListItem > SecretTypes { get; set; }

        protected IApiResourceAppService ApiResourceAppService { get; }


        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            ApiResourceSecret.ForeignKeyId = foreignKeyId;
            SecretTypes.Add(new SelectListItem("Shared Secret" ,IdentityServerConstants.SecretTypes.SharedSecret ));
            SecretTypes.Add(new SelectListItem("X509 Thumbprint",IdentityServerConstants.SecretTypes.X509CertificateThumbprint));
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            await ApiResourceAppService.AddSecretAsync(ApiResourceSecret.ForeignKeyId, new ApiResourceSecretCreateInput
            {
                Type = ApiResourceSecret.Type,
                Description = ApiResourceSecret.Description,
                Value = ApiResourceSecret.Value,
                Expiration = ApiResourceSecret.Expiration
            });

            return NoContent();
        }

        public class ApiResourceSecretViewModel: ApiResourceSecretCreateInput
        {
            public Guid ForeignKeyId { get; set; }
        }


    }
}
