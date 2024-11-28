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
        await CreateFolderAsync(ResourceConsts.PublicFolder.Name, ResourceConsts.PublicFolder.DefaultFileTypes,
            ResourceConsts.PublicFolder.DefaultFileMaxSize, ResourceConsts.PublicFolder.DefaultQuota, tenantId);

        Logger.LogInformation($"Creating shared root folder.");
        await CreateFolderAsync(ResourceConsts.PublicFolder.Name, ResourceConsts.SharedFolder.DefaultFileTypes,
            ResourceConsts.SharedFolder.DefaultFileMaxSize, ResourceConsts.SharedFolder.DefaultQuota, tenantId);

        Logger.LogInformation($"Creating user root folder.");
        await CreateFolderAsync(ResourceConsts.UserFolder.Name, ResourceConsts.UserFolder.DefaultFileTypes,
            ResourceConsts.UserFolder.DefaultFileMaxSize, ResourceConsts.UserFolder.DefaultQuota, tenantId);

        Logger.LogInformation($"Creating virtual path rood folder.");
        await CreateFolderAsync(ResourceConsts.VirtualPath.Name, ResourceConsts.VirtualPath.DefaultFileTypes,
            ResourceConsts.VirtualPath.DefaultFileMaxSize, ResourceConsts.VirtualPath.DefaultQuota, tenantId);

        Logger.LogInformation($"Resource data seed completed.");
    }

    protected virtual async Task CreateFolderAsync(
        string name,
        string fileTypes,
        string maxFileSize,
        string quota,
        Guid? tenantId = null)
    {
        var type = name == ResourceConsts.VirtualPath.Name ? ResourceType.Folder : ResourceType.VirtualPath;
        var resource = new Resource(GuidGenerator.Create(), name, type, true, tenantId);
        resource.SetAllowedFileTypes(fileTypes);
        resource.SetMaxFileSize(maxFileSize.ParseToBytes());
        resource.SetQuota(quota.ParseToBytes());
        await ResourceManager.CreateAsync(resource);
    }
}