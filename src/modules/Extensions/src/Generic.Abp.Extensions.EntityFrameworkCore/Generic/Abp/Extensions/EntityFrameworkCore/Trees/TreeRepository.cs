using Generic.Abp.Extensions.Trees;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.Extensions.EntityFrameworkCore.Trees;

public class TreeRepository<TDbContext, TEntity>(IDbContextProvider<TDbContext> dbContextProvider)
    : EfCoreRepository<TDbContext, TEntity, Guid>(dbContextProvider), ITreeRepository<TEntity>
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