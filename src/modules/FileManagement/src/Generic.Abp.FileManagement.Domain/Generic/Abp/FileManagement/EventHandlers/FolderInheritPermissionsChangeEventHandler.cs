using System.Threading.Tasks;
using Generic.Abp.FileManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Generic.Abp.FileManagement.EventHandlers;

public class FolderInheritPermissionsChangeEventHandler :
    IDistributedEventHandler<FolderInheritPermissionsChangeEto>,
    ITransientDependency
{
    public Task HandleEventAsync(FolderInheritPermissionsChangeEto eventData)
    {
        throw new System.NotImplementedException();
    }
}