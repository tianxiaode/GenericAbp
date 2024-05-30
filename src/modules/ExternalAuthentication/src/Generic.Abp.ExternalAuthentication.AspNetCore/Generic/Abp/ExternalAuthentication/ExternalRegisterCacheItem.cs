using Volo.Abp.Caching;

namespace Generic.Abp.ExternalAuthentication;

[Serializable]
[CacheName("ExternalRegister")]
public class ExternalRegisterCacheItem
{
    public string Provider { get; set; }
    public string ProviderKey { get; set; }

    public ExternalRegisterCacheItem(string provider, string providerKey)
    {
        Provider = provider;
        ProviderKey = providerKey;
    }
}