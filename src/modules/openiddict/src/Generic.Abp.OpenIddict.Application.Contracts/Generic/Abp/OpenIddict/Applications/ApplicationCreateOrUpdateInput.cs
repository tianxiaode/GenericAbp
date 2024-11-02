using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.Validation;

namespace Generic.Abp.OpenIddict.Applications
{
    [Serializable]
    public class ApplicationCreateOrUpdateInput : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(OpenIddictApplicationConsts),
            nameof(OpenIddictApplicationConsts.ClientIdMaxLength))]
        [DisplayName("Application:ClientId")]
        public string ClientId { get; set; } = default!;

        [DisplayName("Application:ClientSecret")]
        public string? ClientSecret { get; set; } = default!;

        [DisplayName("Application:ConsentType")]
        public string ConsentType { get; set; } = default!;

        [Required]
        [DisplayName("Application:DisplayName")]
        public string DisplayName { get; set; } = default!;

        [DisplayName("Application:Permissions")]
        public HashSet<string> Permissions { get; set; } = default!;

        [DisplayName("Application:PostLogoutRedirectUris")]
        public HashSet<Uri> PostLogoutRedirectUris { get; set; } = default!;

        // [DisplayName("Application:Properties")]
        // public Dictionary<string, object> Properties { get; set; } = default!;

        [DisplayName("Application:RedirectUris")]
        public HashSet<Uri> RedirectUris { get; set; } = default!;

        // [DisplayName("Application:Requirements")]
        // public HashSet<string> Requirements { get; set; } = default!;

        [DisplayName("Application:ApplicationType")]
        public string ApplicationType { get; set; } = default!;

        [DisplayName("Application:ClientType")]
        public string ClientType { get; set; } = default!;

        [DisplayName("Application:Settings")] public Dictionary<string, string> Settings { get; set; } = default!;
        [DisplayName("Application:ClientUri")] public string? ClientUri { get; set; } = default!;

        [DisplayName("Application:LogoUri")] public string? LogoUri { get; set; } = default!;
    }
}