using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Folders;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class FolderInheritPermissionsChangeEventHandler(FolderManager folderManager) :
    IDistributedEventHandler<FolderInheritPermissionsChangeEto>,
    ITransientDependency
{
    protected FolderManager FolderManager { get; } = folderManager;

    public async Task HandleEventAsync(FolderInheritPermissionsChangeEto eventData)
    {
        if (!eventData.InheritPermissions)
        {
            return;
        }

        var folder = await FolderManager.GetAsync(eventData.Id);
        if (!folder.ParentId.HasValue)
        {
            return;
        }

        var parent = await FolderManager.GetAsync(folder.ParentId.Value);
        await FolderManager.SetPermissionsAsync(folder, await FolderManager.GetPermissionsAsync(parent));
    }
}