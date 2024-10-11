using Generic.Abp.ExternalAuthentication.dtos;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class MicrosoftAccountAuthenticationProviderHandler : IExternalAuthenticationProviderHandler
{
    public MicrosoftAccountAuthenticationProviderHandler(IOptionsMonitorCache<MicrosoftAccountOptions> optionsCache)
    {
        OptionsCache = optionsCache;
    }

    public string Scheme => MicrosoftAccountDefaults.AuthenticationScheme;

    protected IOptionsMonitorCache<MicrosoftAccountOptions> OptionsCache { get; }

    public Task UpdateOptionsAsync(ExternalProviderDto? provider)
    {
        if (provider == null || string.IsNullOrEmpty(provider.ClientId) || string.IsNullOrEmpty(provider.ClientSecret))
        {
            return Task.CompletedTask;
        }

        OptionsCache.TryRemove(MicrosoftAccountDefaults.AuthenticationScheme);
        OptionsCache.TryAdd(MicrosoftAccountDefaults.AuthenticationScheme, new MicrosoftAccountOptions()
        {
            ClientId = provider.ClientId,
            ClientSecret = provider.ClientSecret
        });
        return Task.CompletedTask;
    }
}