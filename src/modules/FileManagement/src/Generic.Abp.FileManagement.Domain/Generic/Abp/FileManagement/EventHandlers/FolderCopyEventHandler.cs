using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Folders;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class FolderCopyEventHandler(FolderManager folderManager) : IDistributedEventHandler<FolderCopyEto>,
    ITransientDependency
{
    protected FolderManager FolderManager { get; } = folderManager;

    public async Task HandleEventAsync(FolderCopyEto eventData)
    {
        await FolderManager.MoveFilesAsync(eventData.SourceId, eventData.DestinationId);
    }
}