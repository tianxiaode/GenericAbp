using Volo.Abp.Caching;

namespace Generic.Abp.ExternalAuthentication;

[Serializable]
[CacheName("ExternalRegister")]
public class ExternalRegisterCacheItem
{
    public string Provider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
}