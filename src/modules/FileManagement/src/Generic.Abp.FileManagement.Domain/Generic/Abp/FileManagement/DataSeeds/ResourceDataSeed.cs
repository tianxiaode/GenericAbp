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
        publicRootFolder.SetAllowedFileTypes(ResourceConsts.PublicFolder.DefaultFileTypes);
        publicRootFolder.SetMaxFileSize(ResourceConsts.PublicFolder.DefaultFileMaxSize.ParseToBytes());
        publicRootFolder.SetQuota(ResourceConsts.PublicFolder.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(publicRootFolder);

        var usersRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.UsersRootFolderName, ResourceType.Folder, true,
                tenantId);
        usersRootFolder.SetAllowedFileTypes(ResourceConsts.UserFolder.DefaultFileTypes);
        usersRootFolder.SetMaxFileSize(ResourceConsts.UserFolder.DefaultFileMaxSize.ParseToBytes());
        usersRootFolder.SetQuota(ResourceConsts.UserFolder.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(usersRootFolder);

        var sharedRootFolder =
            new Resource(GuidGenerator.Create(), ResourceConsts.SharedRootFolderName, ResourceType.Folder, true,
                tenantId);
        sharedRootFolder.SetAllowedFileTypes(ResourceConsts.SharedFolder.DefaultFileTypes);
        sharedRootFolder.SetMaxFileSize(ResourceConsts.SharedFolder.DefaultFileMaxSize.ParseToBytes());
        sharedRootFolder.SetQuota(ResourceConsts.SharedFolder.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(sharedRootFolder);

        var virtualRootFolder = new Resource(GuidGenerator.Create(), ResourceConsts.VirtualRootFolderName,
            ResourceType.Folder, true,
            tenantId);
        virtualRootFolder.SetAllowedFileTypes(ResourceConsts.VirtualPath.DefaultFileTypes);
        virtualRootFolder.SetMaxFileSize(ResourceConsts.VirtualPath.DefaultFileMaxSize.ParseToBytes());
        virtualRootFolder.SetQuota(ResourceConsts.VirtualPath.DefaultQuota.ParseToBytes());
        await ResourceManager.CreateAsync(virtualRootFolder);
    }
}