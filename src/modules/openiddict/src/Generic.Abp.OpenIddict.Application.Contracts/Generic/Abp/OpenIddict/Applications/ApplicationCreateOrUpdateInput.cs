using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.Validation;

namespace Generic.Abp.OpenIddict.Applications
{
    public class ApplicationCreateOrUpdateInput
    {
        [Required]
        [DynamicStringLength(typeof(OpenIddictApplicationConsts),
            nameof(OpenIddictApplicationConsts.ClientIdMaxLength))]
        [DisplayName("Application:ClientId")]
        public string ClientId { get; set; }

        [DisplayName("Application:ClientSecret")]
        public string ClientSecret { get; set; }

        [DisplayName("Application:ConsentType")]
        public string ConsentType { get; set; }

        [DisplayName("Application:DisplayName")]
        public string DisplayName { get; set; }

        //public List<string> Permissions { get; set; }

        //public List<string> PostLogoutRedirectUris { get; set; }

        //public List<string> Properties { get; set; }

        //public List<string> RedirectUris { get; set; }

        //public List<string> Requirements { get; set; }

        [DisplayName("Application:ApplicationType")]
        public string ApplicationType { get; set; }

        [DisplayName("Application:ClientType")]
        public string ClientType { get; set; }

        [DisplayName("Application:ClientUri")] public string ClientUri { get; set; }

        [DisplayName("Application:LogoUri")] public string LogoUri { get; set; }
    }
}