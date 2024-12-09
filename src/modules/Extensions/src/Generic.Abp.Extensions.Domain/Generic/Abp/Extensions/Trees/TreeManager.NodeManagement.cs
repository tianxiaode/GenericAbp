using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
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
            foreach (var entity in sources)
            {
                await MoveAsync(entity, target);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
            foreach (var entity in sources)
            {
                await CopyAsync(entity, target?.Id);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

        if (adds.Count > 0)
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
    }

    public virtual async Task CanMoveOrCopyAsync(List<Guid> sourceIds, TEntity? target, bool isMove = true)
    {
        switch (sourceIds.Count)
        {
            case 0:
                throw new AbpException("请选择要移动或复制的节点。");
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
                    throw new AbpException(target == null
                        ? "不能将根目录移动到根目录。"
                        : "不能将节点移动到其子节点。");
                }

                await CanMoveOrCopyAdditionalValidationAsync(sourceIds, target, isMove);
                return;
            }
        }

        // 验证选择的节点数量
        await ValidateMoveOrCopyNodeCountAsync(sourceIds.Count);

        // 验证多节点是否存在父子关系冲突
        if (await Repository.HasParentChildConflictAsync(sourceIds, CancellationToken))
        {
            throw new AbpException("不能同时选择目录及其子节点进行移动。");
        }

        // 验证目标节点是否冲突
        if (target == null)
        {
            // 如果目标是根节点，验证是否存在根目录冲突
            var hasRootConflict = await Repository.AnyAsync(m =>
                sourceIds.Contains(m.Id) && m.ParentId == null, CancellationToken);

            if (hasRootConflict)
            {
                throw new AbpException("不能将根目录的直接子节点移动到根目录。");
            }

            return;
        }

        // 验证目标节点是否冲突
        var hasTargetConflict = await Repository.HasParentChildConflictAsync(sourceIds, target.Code, CancellationToken);
        if (hasTargetConflict)
        {
            throw new AbpException("不能移动目录到其子节点。");
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
            throw new AbpException(
                $"Can not move or copy more than {allowedCount} nodes per request.");
        }
    }

    protected virtual Task<int> GetMoveOrCopyMaxNodeCountPerRequestAsync()
    {
        return Task.FromResult(50);
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