using Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.ExternalAuthentication;

public class ExternalProviderChangedHandler(
    ILogger<ExternalProviderChangedHandler> logger,
    IEnumerable<IExternalAuthenticationProviderHandler> providerHandlers,
    IExternalAuthenticationSettingManager externalAuthenticationSettingManager)
    : IDistributedEventHandler<ExternalProviderChangedEto>,
        ITransientDependency
{
    protected ILogger<ExternalProviderChangedHandler> Logger { get; } = logger;
    protected IEnumerable<IExternalAuthenticationProviderHandler> ProviderHandlers { get; } = providerHandlers;

    protected IExternalAuthenticationSettingManager ExternalAuthenticationSettingManager { get; } =
        externalAuthenticationSettingManager;

    public async Task HandleEventAsync(ExternalProviderChangedEto eventData)
    {
        Logger.LogInformation("Handling external provider changed event, updating options cache.");
        var providers = await ExternalAuthenticationSettingManager.GetProvidersAsync();
        foreach (var handler in ProviderHandlers)
        {
            Logger.LogInformation($"Handling provider {handler.Scheme}.");
            await handler.UpdateOptionsAsync(providers.FirstOrDefault(m => m.Provider == handler.Scheme));
        }

        Logger.LogInformation("Options cache updated.");
    }
}