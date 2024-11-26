using Microsoft.AspNetCore.Authentication.OAuth;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public static class DynamicAuthenticationHandlerExtensions
{
    public static async Task UpdateOptionsAsync<THandler, TOptions>(
        this THandler handler,
        ExternalAuthenticationSettingManager settingManager,
        string scheme,
        TOptions options)
        where THandler : OAuthHandler<TOptions>
        where TOptions : OAuthOptions, new()
    {
        var provider = await settingManager.GetProviderAsync(scheme);
        if (provider.Enabled)
        {
            options.ClientId = provider.ClientId;
            options.ClientSecret = provider.ClientSecret;
        }
    }
}