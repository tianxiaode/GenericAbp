using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Resources;
using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Generic.Abp.FileManagement.DataSeeds;

public class ResourceDataSeed(
    ResourceManager resourceManager,
    IGuidGenerator guidGenerator,
    FileManagementSettingManager settingManager)
    : ITransientDependency, IResourceDataSeed
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;

    public async Task SeedAsync(Guid? tenantId = null)
    {
        var publicFolderSetting = await SettingManager.GetPublicFolderSettingAsync();
        var publicRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.PublicRootFolderName, ResourceType.Folder, true,
                tenantId);
        publicRootFolder.SetAllowedFileTypes(publicFolderSetting.FileTypes);
        publicRootFolder.SetMaxFileSize(publicFolderSetting.FileMaxSize.ParseToBytes());
        publicRootFolder.SetQuota(publicFolderSetting.Quota.ParseToBytes());
        await ResourceManager.CreateAsync(publicRootFolder);

        var userFolderSetting = await SettingManager.GetUserFolderSettingAsync();
        var usersRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.UsersRootFolderName, ResourceType.Folder, true,
                tenantId);
        usersRootFolder.SetAllowedFileTypes(userFolderSetting.FileTypes);
        usersRootFolder.SetMaxFileSize(userFolderSetting.FileMaxSize.ParseToBytes());
        usersRootFolder.SetQuota(userFolderSetting.Quota.ParseToBytes());
        await ResourceManager.CreateAsync(usersRootFolder);

        var sharedFolderSetting = await SettingManager.GetSharedFolderSettingAsync();
        var sharedRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.SharedRootFolderName, ResourceType.Folder, true,
                tenantId);
        sharedRootFolder.SetAllowedFileTypes(sharedFolderSetting.FileTypes);
        sharedRootFolder.SetMaxFileSize(sharedFolderSetting.FileMaxSize.ParseToBytes());
        sharedRootFolder.SetQuota(sharedFolderSetting.Quota.ParseToBytes());
        await ResourceManager.CreateAsync(sharedRootFolder);

        var virtualPathSetting = await SettingManager.GetVirtualPathSettingAsync();
        var virtualRootFolder = new Resource(GuidGenerator.Create(), ResourceConsts.VirtualRootFolderName,
            ResourceType.Folder, true,
            tenantId);
        virtualRootFolder.SetAllowedFileTypes(virtualPathSetting.FileTypes);
        virtualRootFolder.SetMaxFileSize(virtualPathSetting.FileMaxSize.ParseToBytes());
        virtualRootFolder.SetQuota(virtualPathSetting.Quota.ParseToBytes());
        await ResourceManager.CreateAsync(virtualRootFolder);
    }
}