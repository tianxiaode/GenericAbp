using System;
using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Jobs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class FileCleanupSettingChangeEventHandler(
    CleanupJobManager cleanupJobManager,
    ILogger<FileCleanupSettingChangeEventHandler> logger) :
    IDistributedEventHandler<FileCleanupSettingChangeEto>,
    ITransientDependency
{
    protected CleanupJobManager CleanupJobManager { get; } = cleanupJobManager;
    protected ILogger<FileCleanupSettingChangeEventHandler> Logger { get; } = logger;

    public virtual async Task HandleEventAsync(FileCleanupSettingChangeEto eventData)
    {
        Logger.LogInformation("Starting file cleanup job.");
        try
        {
            if (eventData.DefaultFileUpdateEnabledChanged || eventData.DefaultFileCleanupEnabledChanged ||
                eventData.TemporaryFileCleanupEnabledChanged || eventData.TemporaryFileCleanupEnabledChanged)
            {
                Logger.LogInformation(
                    $"Starting file cleanup job: {System.Text.Json.JsonSerializer.Serialize(eventData)}");
                await CleanupJobManager.StartAsync(eventData.TenantId);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occurred while starting file cleanup job.");
            throw;
        }

        Logger.LogInformation("File cleanup job completed.");
    }
}