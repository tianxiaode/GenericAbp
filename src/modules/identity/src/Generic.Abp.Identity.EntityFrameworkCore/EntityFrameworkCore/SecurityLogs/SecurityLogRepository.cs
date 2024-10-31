using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Identity.SecurityLogs;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Generic.Abp.Identity.EntityFrameworkCore.SecurityLogs;

public class SecurityLogRepository : EfCoreRepository<IdentityDbContext, IdentitySecurityLog, Guid>,
    ISecurityLogRepository
{
    public SecurityLogRepository(IDbContextProvider<IdentityDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }


    public virtual async Task<long> GetCountAsync(Expression<Func<IdentitySecurityLog, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<IdentitySecurityLog>> GetListAsync(
        Expression<Func<IdentitySecurityLog, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(IdentitySecurityLog.CreationTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllApplicationNamesAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.ApplicationName)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllIdentitiesAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.Identity)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllActionsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.Action)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllUserNamesAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.UserName)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllClientIdsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.ClientId)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllCorrelationIdsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.CorrelationId)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual Task<Expression<Func<IdentitySecurityLog, bool>>> BuildPredicateAsync(
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? applicationName,
        string? identity,
        string? action,
        Guid? userId,
        string? userName,
        string? clientId,
        string? correlationId
    )

    {
        Expression<Func<IdentitySecurityLog, bool>> predicate = p => true;
        if (!string.IsNullOrEmpty(filter))
        {
            predicate = predicate.AndIfNotTrue(m =>
                EF.Functions.Like(m.ApplicationName, $"%{filter}%")
                || EF.Functions.Like(m.Identity, $"%{filter}%")
                || EF.Functions.Like(m.Action, $"%{filter}%")
                || EF.Functions.Like(m.UserName, $"%{filter}%")
                || EF.Functions.Like(m.ClientId, $"%{filter}%")
                || EF.Functions.Like(m.CorrelationId, $"%{filter}%"));
        }

        if (startTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= startTime);
        }

        if (endTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= endTime);
        }

        if (userId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.UserId == userId);
        }

        if (!string.IsNullOrEmpty(applicationName))
        {
            predicate = predicate.AndIfNotTrue(m => m.ApplicationName == applicationName);
        }

        if (!string.IsNullOrEmpty(identity))
        {
            predicate = predicate.AndIfNotTrue(m => m.Identity == identity);
        }

        if (!string.IsNullOrEmpty(action))
        {
            predicate = predicate.AndIfNotTrue(m => m.Action == action);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            predicate = predicate.AndIfNotTrue(m => m.UserName == userName);
        }

        if (!string.IsNullOrEmpty(clientId))
        {
            predicate = predicate.AndIfNotTrue(m => m.ClientId == clientId);
        }

        if (!string.IsNullOrEmpty(correlationId))
        {
            predicate = predicate.AndIfNotTrue(m => m.CorrelationId == correlationId);
        }

        return Task.FromResult(predicate);
    }
}