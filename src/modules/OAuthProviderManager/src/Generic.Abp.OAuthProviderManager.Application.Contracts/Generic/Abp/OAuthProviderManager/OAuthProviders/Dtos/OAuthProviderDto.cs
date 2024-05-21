using System;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;

[Serializable]
public class OAuthProviderDto
{
    public string Provider { get; set; }
    public string DisplayName { get; set; }
    public bool Enabled { get; set; }
}