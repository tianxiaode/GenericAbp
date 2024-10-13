namespace Generic.Abp.ExternalAuthentication.dtos;

[Serializable]
public class ExternalProviderDto
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Provider { get; set; }
    public string DisplayName { get; set; }
    public bool Enabled { get; set; }

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