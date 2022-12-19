using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients
{
    public class ClientIdentityProviderRestrictionDeleteInput
    {
        [Required]
        [DynamicStringLength(typeof(ClientIdPRestrictionConsts), nameof(ClientIdPRestrictionConsts.ProviderMaxLength))]
        [DisplayName("IdentityProviderRestriction:Provider")]

        public string Provider { get; set; }
    }
}
