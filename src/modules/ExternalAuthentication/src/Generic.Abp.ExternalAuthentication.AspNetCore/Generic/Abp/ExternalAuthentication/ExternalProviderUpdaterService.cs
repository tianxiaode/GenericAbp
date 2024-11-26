using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.ExternalAuthentication;

public class ExternalProviderUpdaterService(
    IDistributedEventBus eventBus,
    ILogger<ExternalProviderUpdaterService> logger) : IHostedService
{
    protected IDistributedEventBus EventBus { get; } = eventBus;
    protected ILogger<ExternalProviderUpdaterService> Logger { get; } = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Publishing external provider changed event...");
        try
        {
            await EventBus.PublishAsync(new ExternalProviderChangedEto());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to publish event");
        }

        Logger.LogInformation("External provider changed event published.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}