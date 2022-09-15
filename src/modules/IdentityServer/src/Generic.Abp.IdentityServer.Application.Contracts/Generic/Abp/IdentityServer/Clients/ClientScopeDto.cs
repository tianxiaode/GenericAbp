using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientScopeDto
{
    public string Scope { get; set; }
}