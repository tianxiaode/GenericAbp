using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Resources;
using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.FileManagement.DataSeeds;

public class ResourceDataSeed(
    ResourceManager resourceManager,
    IGuidGenerator guidGenerator,
    FileManagementSettingManager settingManager,
    ILogger<ResourceDataSeed> logger)
    : ITransientDependency, IResourceDataSeed
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;
    protected ILogger<ResourceDataSeed> Logger { get; } = logger;


    public async Task SeedAsync(Guid? tenantId = null)
    {
        Logger.LogInformation($"Executing resource data seed...");

        Logger.LogInformation($"Creating public root folder.");
        await ResourceManager.GetOrCreatePublicRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating shared root folder.");
        await ResourceManager.GetOrCreateSharedRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating user root folder.");
        await ResourceManager.GetOrCreateUsersRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating participant isolationFolder path rood folder.");
        await ResourceManager.CreateParticipantIsolationRootFolderAsync(tenantId);

        Logger.LogInformation($"Resource data seed completed.");
    }
}