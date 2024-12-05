using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

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
        await ResourceManager.GetPublicRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating shared root folder.");
        await ResourceManager.GetSharedRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating users root folder.");
        await ResourceManager.GetUsersRootFolderAsync(tenantId);

        Logger.LogInformation($"Creating participant isolationFolder path rood folder.");
        await ResourceManager.GetParticipantIsolationsRootFolderAsync(tenantId);

        Logger.LogInformation($"Resource data seed completed.");

        //Todo: Add a permission module for  file management
    }
}