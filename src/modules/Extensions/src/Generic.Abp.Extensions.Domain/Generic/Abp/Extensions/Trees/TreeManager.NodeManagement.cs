using Generic.Abp.Extensions.Exceptions.Trees;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository, TResource>
{
    /// <summary>
    /// 移动单个节点
    /// </summary>
    /// <param name="id">要移动的节点Id</param>
    /// <param name="targetId">目标节点id</param>
    /// <returns></returns>
    public virtual async Task MoveAsync(Guid id, Guid? targetId)
    {
        var source = await Repository.GetAsync(id, false, CancellationToken);
        var target = targetId.HasValue ? await Repository.GetAsync(targetId.Value, false, CancellationToken) : null;
        await CanMoveOrCopyAsync(source, target);


        try
        {
            await BeforeMoveAsync(source, target);
            var oldCode = source.Code;
            var newCode = await GetNextChildCodeAsync(target?.Id);
            source.SetCode(newCode);
            await UpdateAsync(source);
            await Repository.MoveByCodeAsync(oldCode, newCode, CancellationToken);

            await AfterMoveAsync(source, target);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Move {source} to {target}", source.Name, target?.Name ?? targetId?.ToString());
            throw;
        }
    }

    /// <summary>
    /// 复制单个节点
    /// </summary>
    /// <param name="sourceId"> 要复制的节点Id </param>
    /// <param name="targetId"> 目标节点id </param>
    /// <returns></returns>
    public virtual async Task CopyAsync(Guid sourceId, Guid? targetId)
    {
        var source = await Repository.GetAsync(sourceId, false, CancellationToken);
        var target = targetId.HasValue ? await Repository.GetAsync(targetId.Value, false, CancellationToken) : null;
        await CanMoveOrCopyAsync(source, target);
        try
        {
            await BeforeCopyAsync(source, target);
            var newEntity = await CloneAsync(source);
            newEntity.MoveTo(target?.Id);
            await CreateAsync(newEntity);

            var children = await Repository.GetAllChildrenByCodeAsync(source.Code, CancellationToken);
            var adds = new List<TEntity>();
            await CopyChildrenAsync(source, newEntity, children, adds);

            if (adds.Count > 0)
            {
                await Repository.InsertManyAsync(adds, true, CancellationToken);
            }

            await AfterCopyAsync(source, newEntity, target);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Move {source} to {target}", source.Name, target?.Name ?? targetId?.ToString());
            throw;
        }
    }


    /// <summary>
    /// 复制子节点
    /// </summary>
    /// <param name="source"> 要复制的节点 </param>
    /// <param name="target"> 目标节点 </param>
    /// <param name="allChildren"> 所有子节点 </param>
    /// <param name="adds"> 新增的节点 </param>
    /// <returns></returns>
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

    public virtual async Task CanMoveOrCopyAsync(TEntity source, TEntity? target, bool isMove = true)
    {
        // 验证选择的节点是否与目标节点有冲突
        await ValidateHasConflictAsync(source, target, isMove);

        // 验证选择的节点的子节点数量是否超过限制
        await ValidateMoveOrCopyMaxChildNodeCountAsync(source);

        // 验证选择的节点的子节点名称是否重复
        await ValidateIsDuplicateNameAsync(source, target);

        //附加验证
        await CanMoveOrCopyAdditionalValidationAsync(source, target, isMove);
    }

    protected virtual Task CanMoveOrCopyAdditionalValidationAsync(TEntity source, TEntity? target,
        bool isMove = true)
    {
        return Task.CompletedTask;
    }

    protected virtual Task ValidateHasConflictAsync(TEntity source, TEntity? target, bool isMove = true)
    {
        if (target == null && source.Parent == null)
        {
            throw new CanNotMoveRootToRootBusinessException();
        }

        if (target != null && target.Code.StartsWith(source.Code))
        {
            throw new CanNotMoveToChildNodeBusinessException();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 验证选择的节点的子节点数量是否超过限制
    /// </summary>
    /// <param name="source"> 要移动或复制的节点 </param>
    /// <returns></returns>
    /// <exception cref="CanNotMoveOrCopyMoreThanCountChildNodesBusinessException"></exception>
    protected virtual async Task ValidateMoveOrCopyMaxChildNodeCountAsync(TEntity source)
    {
        var maxChildNodeCount = await GetMoveOrCopyMacChildNodeCountAsync();
        var childNodeCount = await Repository.GetAllChildrenCountByCodeAsync(source.Code, CancellationToken);
        if (childNodeCount > maxChildNodeCount)
        {
            throw new CanNotMoveOrCopyMoreThanCountChildNodesBusinessException(maxChildNodeCount, childNodeCount);
        }
    }

    /// <summary>
    /// 验证选择的节点的子节点名称是否重复
    /// </summary>
    /// <param name="source"> 要移动或复制的节点 </param>
    /// <param name="target"> 目标节点 </param>
    /// <returns></returns>
    /// <exception cref="DuplicateNameException"></exception>
    protected virtual async Task ValidateIsDuplicateNameAsync(TEntity source, TEntity? target)
    {
        if (target == null &&
            await Repository.AnyAsync(m => m.ParentId == null && m.Name.ToLower() == source.Name.ToLower(),
                CancellationToken))
        {
            throw new DuplicateNameException(source.Name);
        }

        if (target != null &&
            await Repository.AnyAsync(m => m.ParentId == target.Id && m.Name.ToLower() == source.Name.ToLower(),
                CancellationToken))
        {
            throw new DuplicateNameException(source.Name);
        }
    }

    /// <summary>
    /// 获取移动或复制节点的最大子节点数量
    /// </summary>
    /// <returns> 最大子节点数量 </returns>
    protected virtual Task<int> GetMoveOrCopyMacChildNodeCountAsync()
    {
        return Task.FromResult(100);
    }

    /// <summary>
    /// 克隆节点
    /// </summary>
    /// <param name="source"> 要克隆的节点 </param>
    /// <returns></returns>
    protected virtual Task<TEntity> CloneAsync(TEntity source)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// 复制节点之前的虚方法
    /// </summary>
    /// <param name="source"> 源节点 </param>
    /// <param name="newParent"> 新父节点 </param>
    /// <returns></returns>
    protected virtual Task BeforeCopyAsync(TEntity source, TEntity? newParent)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 复制节点之后的虚方法
    /// </summary>
    /// <param name="source"> 源节点 </param>
    /// <param name="target"> 目标节点 </param>
    /// <param name="newParent"> 新父节点 </param>
    /// <returns></returns>
    protected virtual Task AfterCopyAsync(TEntity source, TEntity? target, TEntity? newParent)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 移动节点之前的虚方法
    /// </summary>
    /// <param name="source"> 源节点 </param>
    /// <param name="target"> 目标节点 </param>
    /// <returns></returns>
    protected virtual Task BeforeMoveAsync(TEntity source, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }

    /// <summary>
    /// 移动节点之后的虚方法
    /// </summary>
    /// <param name="source"> 源节点 </param>
    /// <param name="target"> 目标节点 </param>
    /// <returns></returns>
    protected virtual Task AfterMoveAsync(TEntity source, TEntity? target)
    {
        // 默认实现为空操作
        return Task.CompletedTask;
    }
}