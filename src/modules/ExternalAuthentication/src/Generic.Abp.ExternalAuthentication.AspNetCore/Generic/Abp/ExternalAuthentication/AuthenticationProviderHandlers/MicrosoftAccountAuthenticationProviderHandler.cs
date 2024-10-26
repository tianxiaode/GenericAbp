using AspNet.Security.OAuth.GitHub;
using Generic.Abp.ExternalAuthentication.dtos;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class MicrosoftAccountAuthenticationProviderHandler : IExternalAuthenticationProviderHandler
{
    public MicrosoftAccountAuthenticationProviderHandler(IOptionsMonitor<MicrosoftAccountOptions> options)
    {
        Options = options;
    }

    public string Scheme => MicrosoftAccountDefaults.AuthenticationScheme;

    protected IOptionsMonitor<MicrosoftAccountOptions> Options { get; }

    public Task UpdateOptionsAsync(ExternalProviderDto? provider)
    {
        if (provider is { Enabled: false } || string.IsNullOrEmpty(provider?.ClientId) ||
            string.IsNullOrEmpty(provider.ClientSecret))
        {
            return Task.CompletedTask;
        }

        var options = Options.Get(GitHubAuthenticationDefaults.AuthenticationScheme);
        options.ClientId = provider.ClientId;
        options.ClientSecret = provider.ClientSecret;
        return Task.CompletedTask;
    }
}