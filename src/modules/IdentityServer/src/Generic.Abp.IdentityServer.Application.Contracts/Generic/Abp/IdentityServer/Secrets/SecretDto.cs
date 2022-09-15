using System;

namespace Generic.Abp.IdentityServer.Secrets;

[Serializable]
public class SecretDto
{
    public string Type { get; set; }

    public string Value { get; set; }

    public string Description { get; set; }

    public DateTime? Expiration { get; set; }

}