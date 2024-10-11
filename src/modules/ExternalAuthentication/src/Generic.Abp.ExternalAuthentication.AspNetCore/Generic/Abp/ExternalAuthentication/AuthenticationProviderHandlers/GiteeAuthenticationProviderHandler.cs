using AspNet.Security.OAuth.Gitee;
using AspNet.Security.OAuth.GitHub;
using Generic.Abp.ExternalAuthentication.dtos;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class GiteeAuthenticationProviderHandler : IExternalAuthenticationProviderHandler
{
    public GiteeAuthenticationProviderHandler(IOptionsMonitorCache<GiteeAuthenticationOptions> optionsCache)
    {
        OptionsCache = optionsCache;
    }

    public string Scheme => GiteeAuthenticationDefaults.AuthenticationScheme;

    protected IOptionsMonitorCache<GiteeAuthenticationOptions> OptionsCache { get; }

    public Task UpdateOptionsAsync(ExternalProviderDto? provider)
    {
        if (provider == null || string.IsNullOrEmpty(provider.ClientId) || string.IsNullOrEmpty(provider.ClientSecret))
        {
            return Task.CompletedTask;
        }

        OptionsCache.TryRemove(GitHubAuthenticationDefaults.AuthenticationScheme);
        OptionsCache.TryAdd(GitHubAuthenticationDefaults.AuthenticationScheme, new GiteeAuthenticationOptions()
        {
            ClientId = provider.ClientId,
            ClientSecret = provider.ClientSecret
        });
        return Task.CompletedTask;
    }
}