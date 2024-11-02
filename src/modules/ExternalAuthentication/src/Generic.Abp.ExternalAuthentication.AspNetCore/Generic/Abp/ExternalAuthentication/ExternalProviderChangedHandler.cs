using Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.ExternalAuthentication;

public class ExternalProviderChangedHandler : IDistributedEventHandler<ExternalProviderChangedEto>,
    ITransientDependency
{
    public ExternalProviderChangedHandler(ILogger<ExternalProviderChangedHandler> logger,
        IEnumerable<IExternalAuthenticationProviderHandler> providerHandlers,
        IExternalAuthenticationSettingManager externalAuthenticationSettingManager)
    {
        Logger = logger;
        ProviderHandlers = providerHandlers;
        ExternalAuthenticationSettingManager = externalAuthenticationSettingManager;
    }

    protected ILogger<ExternalProviderChangedHandler> Logger { get; }
    protected IEnumerable<IExternalAuthenticationProviderHandler> ProviderHandlers { get; }
    protected IExternalAuthenticationSettingManager ExternalAuthenticationSettingManager { get; }

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