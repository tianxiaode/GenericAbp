using Generic.Abp.ExternalAuthentication.dtos;

namespace Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;

public interface IExternalAuthenticationProviderHandler
{
    string Scheme { get; } // The authentication scheme, e.g., "Google"
    Task UpdateOptionsAsync(ExternalProviderDto? provider);
}