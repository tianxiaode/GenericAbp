using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientClaimDto
{
    public string Type { get; set; }
    public string Value { get; set; }
}