using Volo.Abp.Auditing;

namespace Generic.Abp.ExternalAuthentication.dtos;

[Serializable]
public class ExternalProviderDto
{
    [DisableAuditing] public string ClientId { get; set; }

    [DisableAuditing] public string ClientSecret { get; set; }
    public string Provider { get; set; }
    public string DisplayName { get; set; }
    public bool Enabled { get; set; }

    public ExternalProviderDto()
    {
        ClientId = "";
        ClientSecret = "";
        Enabled = true;
        Provider = "";
        DisplayName = "";
    }

    public ExternalProviderDto(string provider, string displayName, string clientId, string clientSecret, bool enabled)
    {
        Provider = provider;
        DisplayName = displayName;
        ClientId = clientId;
        ClientSecret = clientSecret;
        Enabled = enabled;
    }

    public ExternalProviderDto(string provider, string displayName)
    {
        Provider = provider;
        DisplayName = displayName;
        Enabled = true;
        ClientId = "";
        ClientSecret = "";
    }
}