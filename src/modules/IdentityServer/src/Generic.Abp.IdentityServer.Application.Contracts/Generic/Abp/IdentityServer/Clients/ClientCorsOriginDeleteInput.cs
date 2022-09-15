using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientCorsOriginDeleteInput
{
    public string Origin { get; set; }
}