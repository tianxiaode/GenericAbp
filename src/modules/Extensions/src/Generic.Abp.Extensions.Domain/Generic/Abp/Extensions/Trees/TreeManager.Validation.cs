using System.Threading.Tasks;
using System;
using Generic.Abp.Extensions.Exceptions;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository>
{
    public virtual Task ValidateAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    public virtual async Task IsAllowMoveOrCopyAsync(TEntity entity, Guid? parentId)
    {
        if (parentId.HasValue && !await Repository.AnyAsync(m => m.Id == parentId, CancellationToken))
        {
            throw new EntityNotFoundBusinessException(nameof(TEntity), parentId);
        }

        if (entity.Id == parentId || entity.ParentId == parentId)
        {
            throw new CanNotMoveOrCopyToItselfBusinessException();
        }
    }
}