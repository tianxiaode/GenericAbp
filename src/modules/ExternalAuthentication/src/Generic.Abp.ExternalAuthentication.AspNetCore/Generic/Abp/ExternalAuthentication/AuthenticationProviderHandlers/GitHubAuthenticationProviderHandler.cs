using AspNet.Security.OAuth.GitHub;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class GitHubAuthenticationProviderHandler : GitHubAuthenticationHandler
{
    public GitHubAuthenticationProviderHandler(IOptionsMonitor<GitHubAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder,
        ExternalAuthenticationSettingManager externalAuthenticationSettingManager) : base(options, logger, encoder)
    {
        var provider =
            externalAuthenticationSettingManager.GetProviderAsync(GitHubAuthenticationDefaults.AuthenticationScheme)
                .GetAwaiter().GetResult();
        if (!provider.Enabled)
        {
            return;
        }

        options.CurrentValue.ClientId = provider.ClientId;
        options.CurrentValue.ClientSecret = provider.ClientSecret;
    }
}