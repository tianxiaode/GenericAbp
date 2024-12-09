using System.Threading.Tasks;
using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class ResourceDeletedEventHandler(ResourceManager resourceManager)
    : IDistributedEventHandler<ResourceDeletedEto>,
        ITransientDependency
{
    protected ResourceManager ResourceManager { get; } = resourceManager;

    public virtual async Task HandleEventAsync(ResourceDeletedEto eventData)
    {
        var ids = eventData.ResourceIds;
        // var entity = await ResourceManager.GetAsync(eventData.ResourceId);
        // if (entity.Type == ResourceType.File)
        // {
        //     await ResourceManager.DecreaseUsedStorageAsync(entity, entity.FileSize ?? 0);
        //     return;
        // }
        //
        // if (entity.Type != ResourceType.Folder)
        // {
        //     return;
        // }
        //
        // var size = await ResourceManager.GetSumSizeAsync(entity.Code);
        //
        // var parents = await ResourceManager.GetListAsync(m => entity.Code.StartsWith(m.Code) && m.HasConfiguration);
        //
        // foreach (var parent in parents)
        // {
        //     await ResourceManager.DecreaseUsedStorageAsync(parent, size);
        // }
    }
}