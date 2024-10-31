using Generic.Abp.Extensions.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.AuditLogging.EntityFrameworkCore;

public class AuditLogExtensionRepository : EfCoreRepository<AbpAuditLoggingDbContext, AuditLog, Guid>,
    IAuditLogExtensionRepository
{
    public AuditLogExtensionRepository(IDbContextProvider<AbpAuditLoggingDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(Expression<Func<AuditLog, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<AuditLog>> GetListAsync(
        Expression<Func<AuditLog, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .IncludeDetails(includeDetails)
            .Where(predicate)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(AuditLog.ExecutionTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetDbSetAsync()).AsNoTracking()
            .Where(a => a.ExecutionTime < endDate.AddDays(1) && a.ExecutionTime > startDate)
            .OrderBy(t => t.ExecutionTime)
            .GroupBy(t => new { t.ExecutionTime.Date })
            .Select(g => new
                { Day = g.Min(t => t.ExecutionTime), avgExecutionTime = g.Average(t => t.ExecutionDuration) })
            .ToListAsync(GetCancellationToken(cancellationToken));

        return result.ToDictionary(element => element.Day.ClearTime(), element => element.avgExecutionTime);
    }

    public virtual async Task<EntityChange> GetEntityChangeAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var entityChange = await (await GetDbContextAsync()).Set<EntityChange>()
            .AsNoTracking()
            .IncludeDetails()
            .Where(x => x.Id == entityChangeId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        if (entityChange == null)
        {
            throw new EntityNotFoundException(typeof(EntityChange));
        }

        return entityChange;
    }

    public virtual async Task<long> GetEntityChangesCoutAsync(
        Expression<Func<EntityChange, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<EntityChange>()
            .Where(predicate).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<EntityChange>> GetEntityChangesAsync(
        Expression<Func<EntityChange, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<EntityChange>()
            .AsNoTracking()
            .IncludeDetails(includeDetails)
            .Where(predicate)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(EntityChange.ChangeTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<EntityChangeWithUsername> GetEntityChangeWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var auditLog = await (await GetDbSetAsync()).AsNoTracking().IncludeDetails()
            .Where(x => x.EntityChanges.Any(y => y.Id == entityChangeId))
            .FirstAsync(GetCancellationToken(cancellationToken));

        return new EntityChangeWithUsername()
        {
            EntityChange = auditLog.EntityChanges.First(x => x.Id == entityChangeId),
            UserName = auditLog.UserName
        };
    }

    public virtual async Task<List<EntityChangeWithUsername>> GetEntityChangesWithUsernameAsync(
        string entityId,
        string entityTypeFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = dbContext.Set<EntityChange>()
            .AsNoTracking()
            .IncludeDetails()
            .Where(x => x.EntityId == entityId && x.EntityTypeFullName == entityTypeFullName);

        return await (from e in query
                join auditLog in dbContext.AuditLogs on e.AuditLogId equals auditLog.Id
                select new EntityChangeWithUsername { EntityChange = e, UserName = auditLog.UserName })
            .OrderByDescending(x => x.EntityChange.ChangeTime).ToListAsync(GetCancellationToken(cancellationToken));
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

    public virtual async Task<List<string>> GetAllUserNamesAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.UserName)
            .Distinct()
            .PageBy(0, 100)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetAllUrlsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Select(m => m.Url)
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

    public virtual Task<Expression<Func<AuditLog, bool>>> BuildPredicateAsync(
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? applicationName,
        string? userName,
        string? url,
        string? clientId,
        string? correlationId,
        int? minExecutionDuration,
        int? maxExecutionDuration
    )
    {
        Expression<Func<AuditLog, bool>> predicate = p => true;
        if (!string.IsNullOrEmpty(filter))
        {
            predicate = predicate.AndIfNotTrue(m =>
                EF.Functions.Like(m.ApplicationName, $"%{filter}%")
                || EF.Functions.Like(m.Url, $"%{filter}%")
                || EF.Functions.Like(m.Exceptions, $"%{filter}%")
                || EF.Functions.Like(m.UserName, $"%{filter}%")
                || EF.Functions.Like(m.ClientId, $"%{filter}%")
                || EF.Functions.Like(m.CorrelationId, $"%{filter}%"));
        }

        if (startTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ExecutionTime >= startTime);
        }

        if (endTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ExecutionTime <= endTime);
        }

        if (!string.IsNullOrEmpty(applicationName))
        {
            predicate = predicate.AndIfNotTrue(m => m.ApplicationName == applicationName);
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

        if (!string.IsNullOrEmpty(url))
        {
            predicate = predicate.AndIfNotTrue(m => m.Url == url);
        }

        if (minExecutionDuration.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ExecutionDuration >= minExecutionDuration);
        }

        if (maxExecutionDuration.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ExecutionDuration <= maxExecutionDuration);
        }

        return Task.FromResult(predicate);
    }

    public virtual Task<Expression<Func<EntityChange, bool>>> BuildPredicateAsync(
        string? filter,
        Guid? auditLogId,
        DateTime? startTime,
        DateTime? endTime,
        EntityChangeType? changeType,
        string? entityId
    )
    {
        Expression<Func<EntityChange, bool>> predicate = p => true;
        if (!string.IsNullOrEmpty(filter))
        {
            predicate = predicate.AndIfNotTrue(m => EF.Functions.Like(m.EntityTypeFullName, $"%{filter}%"));
        }

        if (auditLogId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.AuditLogId == auditLogId);
        }

        if (startTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ChangeTime >= startTime);
        }

        if (endTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ChangeTime <= endTime);
        }

        if (changeType.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.ChangeType == changeType);
        }


        if (!string.IsNullOrEmpty(entityId))
        {
            predicate = predicate.AndIfNotTrue(m => m.EntityId == entityId);
        }


        return Task.FromResult(predicate);
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<AuditLog> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public override async Task<IQueryable<AuditLog>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}