using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientGrantTypeDto
{
    public string GrantType { get; set; }
}