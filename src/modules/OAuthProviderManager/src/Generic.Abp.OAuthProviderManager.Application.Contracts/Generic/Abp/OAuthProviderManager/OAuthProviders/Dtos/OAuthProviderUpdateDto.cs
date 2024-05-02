using System;
using System.ComponentModel;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;

[Serializable]
public class OAuthProviderUpdateDto
{
    [DisplayName("OAuthProvider:OnlyEnabled")]
    public bool Enabled { get; set; }
}