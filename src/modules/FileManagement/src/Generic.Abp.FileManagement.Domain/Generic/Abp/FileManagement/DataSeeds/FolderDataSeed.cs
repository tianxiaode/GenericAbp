using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Folders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Generic.Abp.FileManagement.DataSeeds;

public class FolderDataSeed(FolderManager folderManager, IGuidGenerator guidGenerator)
    : ITransientDependency, IFolderDataSeed
{
    protected FolderManager FolderManager { get; } = folderManager;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;

    public async Task SeedAsync(Guid? tenantId = null)
    {
        var publicRootFolder =
            new Folder(GuidGenerator.Create(), null, FolderConsts.PublicRootFolderName, true, tenantId);
        publicRootFolder.SetAllowedFileTypes("*");
        await FolderManager.CreateAsync(publicRootFolder);

        var usersRootFolder =
            new Folder(GuidGenerator.Create(), null, FolderConsts.UsersRootFolderName, true, tenantId);
        usersRootFolder.SetAllowedFileTypes("*");
        await FolderManager.CreateAsync(usersRootFolder);

        var sharedRootFolder =
            new Folder(GuidGenerator.Create(), null, FolderConsts.SharedRootFolderName, true, tenantId);
        sharedRootFolder.SetAllowedFileTypes("*");
        await FolderManager.CreateAsync(sharedRootFolder);
    }
}