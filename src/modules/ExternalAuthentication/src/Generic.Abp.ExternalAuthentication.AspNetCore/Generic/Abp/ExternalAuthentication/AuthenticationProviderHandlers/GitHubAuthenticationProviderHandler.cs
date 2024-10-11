using AspNet.Security.OAuth.GitHub;
using Generic.Abp.ExternalAuthentication.dtos;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class GitHubAuthenticationProviderHandler : IExternalAuthenticationProviderHandler
{
    public GitHubAuthenticationProviderHandler(IOptionsMonitorCache<GitHubAuthenticationOptions> optionsCache)
    {
        OptionsCache = optionsCache;
    }

    public string Scheme => GitHubAuthenticationDefaults.AuthenticationScheme;

    protected IOptionsMonitorCache<GitHubAuthenticationOptions> OptionsCache { get; }

    public Task UpdateOptionsAsync(ExternalProviderDto? provider)
    {
        if (provider == null || string.IsNullOrEmpty(provider.ClientId) || string.IsNullOrEmpty(provider.ClientSecret))
        {
            return Task.CompletedTask;
        }

        OptionsCache.TryRemove(GitHubAuthenticationDefaults.AuthenticationScheme);
        OptionsCache.TryAdd(GitHubAuthenticationDefaults.AuthenticationScheme, new GitHubAuthenticationOptions()
        {
            ClientId = provider.ClientId,
            ClientSecret = provider.ClientSecret
        });
        return Task.CompletedTask;
    }
}