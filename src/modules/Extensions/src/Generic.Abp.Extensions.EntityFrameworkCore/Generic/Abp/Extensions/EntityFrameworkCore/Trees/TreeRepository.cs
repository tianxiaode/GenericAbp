using Generic.Abp.Extensions.Trees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.Extensions.EntityFrameworkCore.Trees;

public class TreeRepository<TDbContext, TEntity>(
    IDbContextProvider<TDbContext> dbContextProvider)
    : ExtensionRepository<TDbContext, TEntity>(dbContextProvider),
        ITreeRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, ITree<TEntity>
{
    public virtual async Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(m => m.ParentId == id, GetCancellationToken(cancellation));
    }

    public virtual async Task<long> GetAllChildrenCountByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).LongCountAsync(m => m.Code.StartsWith(code + "."),
            GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => EF.Functions.Like(m.Name, $"%{filter}%"))
            .Select(m => m.Code)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TEntity>> GetAllChildrenByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => m.Code.StartsWith(code + "."))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Guid>> GetAllParentIdsByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => code.StartsWith(m.Code + "."))
            .Select(m => m.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TEntity>> GetAllParentByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => code.StartsWith(m.Code + "."))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> MoveByCodeAsync(string oldParentCode, string newParentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var count = 0;
            Logger.LogInformation(
                "Starting update operation for oldParentCode: {OldParentCode}, newParentCode: {NewParentCode}",
                oldParentCode, newParentCode);

            count = await (await GetDbSetAsync()).Where(m => m.Code.StartsWith(oldParentCode + ".")).ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(m => m.Code,
                        m => newParentCode + m.Code.Substring(oldParentCode.Length + 1)),
                cancellationToken: cancellationToken
            );


            Logger.LogInformation(
                "Update operation completed for oldParentCode: {OldParentCode}, newParentCode: {NewParentCode}",
                oldParentCode, newParentCode);
            return count;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Error occurred during update operation for oldParentCode: {OldParentCode}, newParentCode: {NewParentCode}",
                oldParentCode, newParentCode);
            throw;
        }
    }

    public virtual async Task<bool> HasParentChildConflictAsync(List<Guid> sourceIds,
        CancellationToken cancellation = default)
    {
        if (sourceIds.Count == 0)
        {
            return false;
        }

        var dbSet = await GetDbSetAsync();

        // 查询是否存在父子关系冲突
        var hasConflict = await dbSet
            .Where(parent => sourceIds.Contains(parent.Id))
            .AnyAsync(parent => dbSet
                .Where(child => sourceIds.Contains(child.Id) && child.Id != parent.Id)
                .Any(child => child.Code.StartsWith(parent.Code + ".")), cancellation);
        return hasConflict;
    }

    public virtual async Task<bool> HasParentChildConflictAsync(List<Guid> sourceIds, string? targetCode,
        CancellationToken cancellation = default)
    {
        if (sourceIds.Count == 0)
        {
            return false;
        }

        var sources = (await GetDbSetAsync())
            .Where(m => sourceIds.Contains(m.Id));
        // 查询是否存在父子关系冲突
        if (string.IsNullOrEmpty(targetCode))
        {
            return await sources.AnyAsync(m => m.ParentId == null, cancellation);
        }

        return await sources.AnyAsync(
            m => targetCode.StartsWith(m.Code + "."),
            cancellation);
    }

    public virtual async Task<long> GetAllChildrenCountAsync(
        List<Guid> sourceIds,
        CancellationToken cancellationToken = default)
    {
        if (sourceIds.Count == 0)
        {
            return 0L; // 如果没有传入 sourceIds，直接返回 0
        }

        var dbSet = await GetDbSetAsync();


        // 查询包含子节点的父节点的 Code（只选择必要字段以减少数据量）
        var parentCodes = await dbSet
            .Where(parent => sourceIds.Contains(parent.Id) && dbSet.Any(m => m.ParentId == parent.Id))
            .Select(parent => parent.Code)
            .ToListAsync(cancellationToken);

        if (parentCodes.Count == 0)
        {
            return 0L; // 如果没有符合条件的父节点，直接返回 0
        }

        // 直接在数据库中统计每个父节点的子节点数量
        Expression<Func<TEntity, bool>> predicate = m => false; // 初始值为 false
        predicate = parentCodes.Aggregate(predicate,
            (current, code) => System.Linq.PredicateBuilder.Or(current, m => m.Code.StartsWith(code + ".")));

        return await dbSet.Where(predicate)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<int> DeleteAllChildrenByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var count = 0;
            Logger.LogInformation("Starting delete operation for code: {Code}", code);
            count = await (await GetDbSetAsync()).Where(m => m.Code.StartsWith(code + "."))
                .ExecuteDeleteAsync(cancellationToken);
            Logger.LogInformation("Delete operation completed for code: {Code}", code);
            return count;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred during delete operation for code: {Code}", code);
            throw;
        }
    }
}