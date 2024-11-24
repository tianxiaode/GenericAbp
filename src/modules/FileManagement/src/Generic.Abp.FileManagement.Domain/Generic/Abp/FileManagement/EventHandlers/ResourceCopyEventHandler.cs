using Generic.Abp.FileManagement.Events;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class ResourceCopyEventHandler(ResourceManager resourceManager) : IDistributedEventHandler<ResourceCopyEto>,
    ITransientDependency
{
    protected ResourceManager ResourceManager { get; } = resourceManager;

    public Task HandleEventAsync(ResourceCopyEto eventData)
    {
        //TODO: 将源资源的全部子节点移动到目标资源下
        //await ResourceManager.MoveFilesAsync(eventData.SourceId, eventData.DestinationId);
        throw new System.NotImplementedException();
    }
}