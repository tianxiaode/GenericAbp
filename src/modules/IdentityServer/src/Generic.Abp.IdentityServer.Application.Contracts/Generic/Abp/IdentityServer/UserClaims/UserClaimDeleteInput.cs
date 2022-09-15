using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.UserClaims;

[Serializable]
public class UserClaimDeleteInput
{
    [Required]
    [DisplayName("UserClaim:Type")]
    public string Type { get; set; }

}