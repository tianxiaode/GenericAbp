using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository>
{
    public virtual async Task MoveAsync(Guid id, Guid? parentId)
    {
        var entity = await Repository.GetAsync(id, false, CancellationToken);
        if (entity.ParentId == parentId)
        {
            return;
        }

        await MoveAsync(entity, parentId);
    }

    [UnitOfWork]
    public virtual async Task MoveAsync(TEntity entity, Guid? parentId)
    {
        var oldCode = entity.Code;
        var newCode = await GetNextChildCodeAsync(parentId);
        entity.SetCode(newCode);
        await UpdateAsync(entity);
        await Repository.MoveByCodeAsync(oldCode, newCode, CancellationToken);
    }

    public virtual async Task CopyAsync(Guid id, Guid? parentId)
    {
        var entity = await Repository.GetAsync(id, true, CancellationToken);
        await CopyAsync(entity, parentId);
    }

    public virtual async Task CopyAsync(TEntity entity, Guid? parentId)
    {
        var newEntity = await CloneAsync(entity);
        newEntity.MoveTo(parentId);
        await CreateAsync(newEntity);

        var children = await Repository.GetAllChildrenByCodeAsync(entity.Code, CancellationToken);
        var adds = new List<TEntity>();
        await CopyChildrenAsync(entity, newEntity, children, adds);

        if (adds.Any())
        {
            await Repository.InsertManyAsync(adds, true, CancellationToken);
        }

        await AfterCopyAsync(entity, newEntity);
    }

    public virtual async Task CopyChildrenAsync(TEntity source, TEntity target, List<TEntity> allChildren,
        List<TEntity> adds)
    {
        var queue = new Queue<(TEntity Source, TEntity Target)>();
        queue.Enqueue((source, target));

        while (queue.Any())
        {
            var (currentSource, currentTarget) = queue.Dequeue();
            Logger.LogDebug("Copy Children `{0}`: {1} -> {2}", currentSource.Name, currentSource.Code,
                currentTarget.Code);
            // 获取当前节点的直接子节点
            var children = allChildren.Where(m => m.ParentId == currentSource.Id).ToList();

            foreach (var oldChild in children)
            {
                // 克隆并设置父节点信息
                var newChild = await CloneAsync(oldChild);
                newChild.MoveTo(currentTarget.Id);

                // 替换 Code
                newChild.SetCode(currentTarget.Code + oldChild.Code[(currentSource.Code.Length + 1)..]);
                Logger.LogDebug("New Children `{0}`: {1} -> {2}", oldChild.Name, oldChild.Code, newChild.Code);

                // 加入新增列表和队列
                adds.Add(newChild);
                queue.Enqueue((oldChild, newChild));
            }
        }
        // var children = allChildren.Where(m => m.ParentId == source.Id).ToList();
        // if (!children.Any())
        // {
        //     return;
        // }
        //
        // foreach (var old in children)
        // {
        //     var newChild = await CloneAsync(old);
        //     newChild.MoveTo(target.Id);
        //     newChild.SetCode(target.Code + source.Code[(source.Code.Length + 1)..]);
        //     adds.Add(newChild);
        //     await CopyChildrenAsync(old, newChild, allChildren, adds);
        // }
    }

    protected virtual Task<TEntity> CloneAsync(TEntity source)
    {
        throw new NotImplementedException();
    }

    protected virtual Task AfterCopyAsync(TEntity entity, TEntity newEntity)
    {
        return Task.CompletedTask;
    }
}