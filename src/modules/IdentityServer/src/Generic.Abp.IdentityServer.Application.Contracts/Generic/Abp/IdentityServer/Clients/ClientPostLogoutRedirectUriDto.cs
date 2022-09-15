using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientPostLogoutRedirectUriDto
{
    public string PostLogoutRedirectUri { get; set; }
}