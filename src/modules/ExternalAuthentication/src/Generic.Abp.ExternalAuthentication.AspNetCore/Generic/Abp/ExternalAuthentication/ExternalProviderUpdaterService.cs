namespace Generic.Abp.ExternalAuthentication;

public class ExternalProviderUpdaterService : IHostedService
{
    public ExternalProviderUpdaterService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected IServiceProvider ServiceProvider { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = ServiceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ExternalProviderChangedHandler>();
        await handler.HandleEventAsync(new ExternalProviderChangedEto());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}