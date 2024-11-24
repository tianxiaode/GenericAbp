using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Resources;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Generic.Abp.FileManagement.DataSeeds;

public class ResourceDataSeed(ResourceManager resourceManager, IGuidGenerator guidGenerator)
    : ITransientDependency, IResourceDataSeed
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;

    public async Task SeedAsync(Guid? tenantId = null)
    {
        var publicRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.PublicRootFolderName, ResourceType.Folder, true,
                tenantId);
        publicRootFolder.SetAllowedFileTypes(ResourceConsts.Public.DefaultFileTypes);
        publicRootFolder.SetMaxFileSize(ResourceConsts.Public.DefaultFileMaxSize.ParseToBytes());
        publicRootFolder.SetQuota(ResourceConsts.Public.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(publicRootFolder);

        var usersRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.UsersRootFolderName, ResourceType.Folder, true,
                tenantId);
        usersRootFolder.SetAllowedFileTypes(ResourceConsts.User.DefaultFileTypes);
        usersRootFolder.SetMaxFileSize(ResourceConsts.User.DefaultFileMaxSize.ParseToBytes());
        usersRootFolder.SetQuota(ResourceConsts.User.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(usersRootFolder);

        var sharedRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.SharedRootFolderName, ResourceType.Folder, true,
                tenantId);
        sharedRootFolder.SetAllowedFileTypes(ResourceConsts.Shared.DefaultFileTypes);
        sharedRootFolder.SetMaxFileSize(ResourceConsts.Shared.DefaultFileMaxSize.ParseToBytes());
        sharedRootFolder.SetQuota(ResourceConsts.Shared.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(sharedRootFolder);

        var virtualRootFolder = new Resource(GuidGenerator.Create(), ResourceConsts.VirtualRootFolderName,
            ResourceType.Folder, true,
            tenantId);
        virtualRootFolder.SetAllowedFileTypes(ResourceConsts.Virtual.DefaultFileTypes);
        virtualRootFolder.SetMaxFileSize(ResourceConsts.Virtual.DefaultFileMaxSize.ParseToBytes());
        virtualRootFolder.SetQuota(ResourceConsts.Virtual.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(virtualRootFolder);
    }
}