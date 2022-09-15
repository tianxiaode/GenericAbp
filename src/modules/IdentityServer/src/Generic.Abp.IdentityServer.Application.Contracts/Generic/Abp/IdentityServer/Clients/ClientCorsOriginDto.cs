using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientCorsOriginDto
{
    public string Origin { get; set; }
}