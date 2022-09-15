using System;

namespace Generic.Abp.IdentityServer.UserClaims;

[Serializable]
public class UserClaimDto
{
    public string Type { get; set; }
}