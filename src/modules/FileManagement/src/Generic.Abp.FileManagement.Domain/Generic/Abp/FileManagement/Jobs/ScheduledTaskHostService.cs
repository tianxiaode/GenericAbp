using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Jobs;

public class ScheduledTaskHostService : IHostedService
{
    protected ILogger<ScheduledTaskHostService> Logger { get; }
    protected CleanupJobManager CleanupJobManager { get; }
    protected ICurrentTenant Tenant { get; }
    protected ITenantStore TenantStore { get; }
    protected AbpMultiTenancyOptions AbpMultiTenancyOptions { get; }

    public ScheduledTaskHostService(
        ILogger<ScheduledTaskHostService> logger,
        CleanupJobManager cleanupJobManager,
        ICurrentTenant currentTenant,
        ITenantStore tenantStore,
        IOptions<AbpMultiTenancyOptions> options,
        IHostApplicationLifetime applicationLifetime)
    {
        Logger = logger;
        CleanupJobManager = cleanupJobManager;
        Tenant = currentTenant;
        TenantStore = tenantStore;
        AbpMultiTenancyOptions = options.Value;

        applicationLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () => await StartAsync(CancellationToken.None));
        });
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting scheduled task initialization...");

        try
        {
            List<Guid?> tenantIds = [null];

            if (AbpMultiTenancyOptions.IsEnabled)
            {
                tenantIds.AddRange(
                    (await TenantStore.GetListAsync()).Select(t => (Guid?)t.Id));
            }

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(tenantIds, parallelOptions, async (tenantId, token) =>
            {
                try
                {
                    using (Tenant.Change(tenantId))
                    {
                        Logger.LogInformation("Applying task settings for TenantId: {TenantId}", tenantId);
                        await CleanupJobManager.StartAsync(tenantId);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error initializing tasks for TenantId: {TenantId}", tenantId);
                }
            });

            Logger.LogInformation("Scheduled task initialization completed.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred during scheduled task initialization.");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("ScheduledTaskHostService is stopping.");
        await CleanupJobManager.StopAllAsync(cancellationToken); // 假设 CleanupJobManager 有此方法
    }
}