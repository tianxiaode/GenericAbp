using System;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;

[Serializable]
public class OAuthProviderGetListInput
{
    public bool OnlyEnabled { get; set; } = false;
}