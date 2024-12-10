using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Exceptions.Trees;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository, TResource>
{
    public virtual async Task MoveAsync(List<Guid> sourceIds, Guid? targetId)
    {
        var target = targetId.HasValue ? await Repository.GetAsync(targetId.Value, false, CancellationToken) : null;
        await CanMoveOrCopyAsync(sourceIds, target);


        var sources = await Repository.GetListAsync(m => sourceIds.Contains(m.Id), false, CancellationToken);
        try
        {
            await BeforeMoveAllAsync(sources, target);
            foreach (var entity in sources)
            {
                await MoveAsync(entity, target);
            }

            await AfterMoveAllAsync(sources, target);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Move {sourceIds} to {targetId}", sourceIds, targetId);
            throw;
        }
    }

    // 定义在所有移动操作之前的虚方法
    protected virtual Task BeforeMoveAllAsync(IEnumerable<TEntity> sources, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }

// 定义在所有移动操作之后的虚方法
    protected virtual Task AfterMoveAllAsync(IEnumerable<TEntity> sources, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }

    public virtual async Task MoveAsync(TEntity entity, TEntity? target)
    {
        var oldCode = entity.Code;
        var newCode = await GetNextChildCodeAsync(target?.Id);
        entity.SetCode(newCode);
        await UpdateAsync(entity);
        await Repository.MoveByCodeAsync(oldCode, newCode, CancellationToken);
    }

    public virtual async Task CopyAsync(List<Guid> sourceIds, Guid? targetId)
    {
        var target = targetId.HasValue ? await Repository.GetAsync(targetId.Value, false, CancellationToken) : null;
        await CanMoveOrCopyAsync(sourceIds, target);


        var sources = await Repository.GetListAsync(m => sourceIds.Contains(m.Id), false, CancellationToken);
        try
        {
            await BeforeCopyAllAsync(sources, target);
            foreach (var entity in sources)
            {
                await CopyAsync(entity, target);
            }

            await AfterCopyAllAsync(sources, target);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Copy {sourceIds} to {targetId}", sourceIds, targetId);
            throw;
        }
    }

// 定义在所有移动操作之前的虚方法
    protected virtual Task BeforeCopyAllAsync(IEnumerable<TEntity> sources, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }

// 定义在所有移动操作之后的虚方法
    protected virtual Task AfterCopyAllAsync(IEnumerable<TEntity> sources, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }

    public virtual async Task CopyAsync(TEntity entity, TEntity? newParent)
    {
        await BeforeCopyAsync(entity, newParent);
        var newEntity = await CloneAsync(entity);
        newEntity.MoveTo(newParent?.Id);
        await CreateAsync(newEntity);

        var children = await Repository.GetAllChildrenByCodeAsync(entity.Code, CancellationToken);
        var adds = new List<TEntity>();
        await CopyChildrenAsync(entity, newEntity, children, adds);

        if (adds.Count > 0)
        {
            await Repository.InsertManyAsync(adds, true, CancellationToken);
        }

        await AfterCopyAsync(entity, newEntity, newParent);
    }

    public virtual async Task CopyChildrenAsync(TEntity source, TEntity target, List<TEntity> allChildren,
        List<TEntity> adds)
    {
        var queue = new Queue<(TEntity Source, TEntity Target)>();
        queue.Enqueue((source, target));

        while (queue.Count > 0)
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
    }

    public virtual async Task CanMoveOrCopyAsync(List<Guid> sourceIds, TEntity? target, bool isMove = true)
    {
        switch (sourceIds.Count)
        {
            case 0:
                throw new NoSelectedItemFoundBusinessException();
            case 1:
            {
                var sourceId = sourceIds[0];

                // 验证单节点是否存在根目录冲突或子节点冲突
                var hasConflict = await Repository.AnyAsync(m =>
                    m.Id == sourceId && (
                        (target == null && m.ParentId == null) || // 根目录冲突
                        (target != null && target.Code.StartsWith(m.Code)) // 子节点冲突
                    ), CancellationToken);

                if (hasConflict)
                {
                    throw (target == null
                        ? new CanNotMoveRootToRootBusinessException()
                        : new CanNotMoveToChildNodeBusinessException());
                }

                await CanMoveOrCopyAdditionalValidationAsync(sourceIds, target, isMove);
                return;
            }
        }

        // 验证选择的节点数量
        await ValidateMoveOrCopyNodeCountAsync(sourceIds.Count);

        // 验证选择的节点的子节点数量是否超过限制
        await ValidateMoveOrCopyMaxChildNodeCountAsync(sourceIds);

        // 验证多节点是否存在父子关系冲突
        if (await Repository.HasParentChildConflictAsync(sourceIds, CancellationToken))
        {
            throw new CanNotMoveParentAndItsChildrenBusinessException();
        }

        // 验证目标节点是否冲突
        if (target == null)
        {
            // 如果目标是根节点，验证是否存在根目录冲突
            var hasRootConflict = await Repository.AnyAsync(m =>
                sourceIds.Contains(m.Id) && m.ParentId == null, CancellationToken);

            if (hasRootConflict)
            {
                throw new CanNotMoveRootToRootBusinessException();
            }

            return;
        }

        // 验证目标节点是否冲突
        var hasTargetConflict = await Repository.HasParentChildConflictAsync(sourceIds, target.Code, CancellationToken);
        if (hasTargetConflict)
        {
            throw new CanNotMoveParentAndItsChildrenBusinessException();
        }

        await CanMoveOrCopyAdditionalValidationAsync(sourceIds, target, isMove);
    }

    protected virtual Task CanMoveOrCopyAdditionalValidationAsync(List<Guid> sourceIds, TEntity? target,
        bool isMove = true)
    {
        return Task.CompletedTask;
    }

    protected virtual async Task ValidateMoveOrCopyNodeCountAsync(int count)
    {
        var allowedCount = await GetMoveOrCopyMaxNodeCountPerRequestAsync();
        if (count > allowedCount)
        {
            throw new CanNotMoveOrCopyMoreThanCountNodesBusinessException(allowedCount, count);
        }
    }

    protected virtual async Task ValidateMoveOrCopyMaxChildNodeCountAsync(List<Guid> sourceIds)
    {
        var maxChildNodeCount = await GetMoveOrCopyMacChildNodeCountAsync();
        var childNodeCount = await Repository.GetAllChildrenCountAsync(sourceIds, CancellationToken);
        if (childNodeCount > maxChildNodeCount)
        {
            throw new CanNotMoveOrCopyMoreThanCountChildNodesBusinessException(maxChildNodeCount, childNodeCount);
        }
    }

    protected virtual Task<int> GetMoveOrCopyMaxNodeCountPerRequestAsync()
    {
        return Task.FromResult(50);
    }

    protected virtual Task<int> GetMoveOrCopyMacChildNodeCountAsync()
    {
        return Task.FromResult(100);
    }

    protected virtual Task<TEntity> CloneAsync(TEntity source)
    {
        throw new NotImplementedException();
    }

    protected virtual Task BeforeCopyAsync(TEntity entity, TEntity? newParent)
    {
        return Task.CompletedTask;
    }

    protected virtual Task AfterCopyAsync(TEntity entity, TEntity? newEntity, TEntity? newParent)
    {
        return Task.CompletedTask;
    }
}