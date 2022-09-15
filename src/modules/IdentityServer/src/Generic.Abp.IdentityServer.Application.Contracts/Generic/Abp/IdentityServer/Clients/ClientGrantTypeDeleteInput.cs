using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientGrantTypeDeleteInput
{
    public string GrantType { get; set; }
}