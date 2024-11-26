using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public class MicrosoftAccountDynamicAuthenticationHandler(
    IOptionsMonitor<MicrosoftAccountOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ExternalAuthenticationSettingManager externalAuthenticationSettingManager)
    : MicrosoftAccountHandler(options, logger, encoder)
{
    protected ExternalAuthenticationSettingManager ExternalAuthenticationSettingManager { get; } =
        externalAuthenticationSettingManager;

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
    {
        await this.UpdateOptionsAsync(ExternalAuthenticationSettingManager, Scheme.Name,
            Options);
        return await base.ExchangeCodeAsync(context);
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await this.UpdateOptionsAsync(ExternalAuthenticationSettingManager, Scheme.Name,
            Options);
        await base.HandleChallengeAsync(properties);
    }
}