using Generic.Abp.FileManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

public partial class ResourceManager
{
    public virtual async Task ValidateUsedStorageAsync(Resource sourceParent, Resource target, List<Guid> operationIds)
    {
        var parentWithConfiguration = await Repository.GetParentWithConfiguration(target.Code, CancellationToken);
        var quota = parentWithConfiguration.GetStorageQuota();
        var usedStorage = parentWithConfiguration.GetUsedStorage();
        var size = await GetSumSizeAsync(sourceParent.Code, operationIds);
        if (usedStorage + size > quota)
        {
            throw new InsufficientStorageSpaceBusinessException(size, usedStorage, quota);
        }
    }

    public virtual async Task<long> GetSumSizeAsync(string code, List<Guid>? ids)
    {
        ids ??= [];
        return await Repository.SumSizeByCodeAsync(code, ids, CancellationToken);
    }


    protected override Task<Resource> CloneAsync(Resource source)
    {
        var entity = new Resource(GuidGenerator.Create(), source.Name, ResourceType.Folder, source.IsStatic,
            source.TenantId);
        entity.SetFileInfoBase(source.FileInfoBaseId);

        //不复制配置，否则会导致配置副作用，譬如扩容
        entity.SetHasConfiguration(false);

        //不复制权限，否则会导致权限副作用，譬如扩权
        //entity.SetPermissions(source.Permissions);
        entity.SetHasPermissions(false);
        return Task.FromResult(entity);
    }
}