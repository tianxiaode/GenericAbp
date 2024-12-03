using Generic.Abp.Extensions.Exceptions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository, TResource>
{
    public virtual async Task ValidateAsync(TEntity entity)
    {
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant(), cancellationToken: CancellationToken))
        {
            throw new DuplicateWarningBusinessException(L[nameof(TEntity)], entity.Name);
        }
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