using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.Clients;

public class ClientCorsOriginCreateInput
{
    [Required]
    [DynamicStringLength(typeof(ClientCorsOriginConsts),nameof(ClientCorsOriginConsts.OriginMaxLength))]
    [DisplayName("ClientCorsOrigin:Origin")]
    public string Origin { get; set; }
}