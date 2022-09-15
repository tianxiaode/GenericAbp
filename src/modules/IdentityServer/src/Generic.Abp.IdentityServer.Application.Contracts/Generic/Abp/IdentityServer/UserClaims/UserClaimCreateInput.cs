using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;
using Volo.Abp.Validation;

namespace Generic.Abp.IdentityServer.UserClaims;

[Serializable]
public class UserClaimCreateInput
{
    [Required]
    [DisplayName("UserClaim:Type")]
    [DynamicStringLength(typeof(UserClaimConsts),nameof(UserClaimConsts.TypeMaxLength))]
    public string Type { get; set; }
}