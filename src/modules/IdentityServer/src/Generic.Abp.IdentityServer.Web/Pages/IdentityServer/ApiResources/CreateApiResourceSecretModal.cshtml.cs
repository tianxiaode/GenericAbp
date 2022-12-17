using Generic.Abp.Enumeration.Validation;
using Generic.Abp.IdentityServer.ApiResources;
using Generic.Abp.IdentityServer.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;
using static Generic.Abp.IdentityServer.IdentityServerConstants;
using System.Xml.Linq;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Generic.Abp.IdentityServer.Secrets;

namespace Generic.Abp.IdentityServer.Web.Pages.IdentityServer.ApiResources
{
    public class CreateApiResourceSecretModalModel : IdentityServerPageModel
    {

        public CreateApiResourceSecretModalModel(IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
            ApiResourceSecret = new ApiResourceSecretViewModel(){};
            SecretTypes = new List<SelectListItem>()
            {
                new SelectListItem("Shared Secret" ,IdentityServerConstants.SecretTypes.SharedSecret ),
                new SelectListItem("X509 Thumbprint",IdentityServerConstants.SecretTypes.X509CertificateThumbprint)
            };
        }

        [BindProperty]
        public ApiResourceSecretViewModel ApiResourceSecret { get; set; }

        [BindProperty]
        public List<SelectListItem > SecretTypes { get; set; }

        protected IApiResourceAppService ApiResourceAppService { get; }


        public async Task<IActionResult> OnGetAsync(Guid foreignKeyId)
        {
            ApiResourceSecret = new ApiResourceSecretViewModel(foreignKeyId);
            return await Task.FromResult(Page()) ;

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
        Logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(ApiResourceSecret));
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

        public class ApiResourceSecretViewModel: SecretCreateInput
        {
            public ApiResourceSecretViewModel()
            {
                Type = IdentityServerConstants.SecretTypes.SharedSecret;
                ForeignKeyId = Guid.NewGuid();
                Value = "";
                Description ="";
            }

            public ApiResourceSecretViewModel(Guid foreignKeyId)
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
