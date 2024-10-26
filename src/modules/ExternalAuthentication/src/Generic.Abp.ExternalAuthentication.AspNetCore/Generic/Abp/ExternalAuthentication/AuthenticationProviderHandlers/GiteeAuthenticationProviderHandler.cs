using AspNet.Security.OAuth.Gitee;
using AspNet.Security.OAuth.GitHub;
using Generic.Abp.ExternalAuthentication.dtos;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class GiteeAuthenticationProviderHandler : IExternalAuthenticationProviderHandler
{
    public GiteeAuthenticationProviderHandler(IOptionsMonitor<GiteeAuthenticationOptions> options)
    {
        Options = options;
    }

    public string Scheme => GiteeAuthenticationDefaults.AuthenticationScheme;

    protected IOptionsMonitor<GiteeAuthenticationOptions> Options { get; }

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