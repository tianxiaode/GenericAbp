using System;
using System.Threading.Tasks;

namespace Generic.Abp.Extensions.Trees;

public partial class TreeManager<TEntity, TRepository, TResource>
{
    public virtual async Task<TEntity> GetAsync(Guid id)
    {
        return await Repository.GetAsync(id, false, CancellationToken);
    }

    public override async Task CreateAsync(TEntity entity, bool autoSave = true)
    {
        entity.SetCode(await GetNextChildCodeAsync(entity.ParentId));
        await ValidateAsync(entity);
        await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
    }

    public override async Task UpdateAsync(TEntity entity, bool autoSave = true)
    {
        await ValidateAsync(entity);
        await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
    }

    public override async Task DeleteAsync(Guid id, bool autoSave = true)
    {
        var entity = await Repository.GetAsync(id, false, CancellationToken);
        await DeleteAsync(entity, autoSave);
    }

    public override async Task DeleteAsync(TEntity entity, bool autoSave = true)
    {
        await Repository.DeleteAllChildrenByCodeAsync(entity.Code, CancellationToken);
        await Repository.DeleteAsync(entity, true, CancellationToken);
    }
}