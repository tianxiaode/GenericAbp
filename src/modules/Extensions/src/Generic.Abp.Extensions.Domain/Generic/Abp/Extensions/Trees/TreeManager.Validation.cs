using Generic.Abp.Extensions.Exceptions;
using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions.Trees;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository, TResource>
{
    public override async Task ValidateAsync(TEntity entity)
    {
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant(), cancellationToken: CancellationToken))
        {
            throw new DuplicateWarningBusinessException(L[nameof(TEntity)], entity.Name);
        }
    }
}