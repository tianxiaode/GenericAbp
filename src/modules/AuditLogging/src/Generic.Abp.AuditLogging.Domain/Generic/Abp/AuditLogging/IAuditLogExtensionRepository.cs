using Generic.Abp.Extensions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;

namespace Generic.Abp.AuditLogging;

public interface IAuditLogExtensionRepository : IExtensionRepository<AuditLog>
{
    Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<EntityChange> GetEntityChangeAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default);

    Task<long> GetEntityChangesCoutAsync(
        Expression<Func<EntityChange, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<EntityChange>> GetEntityChangesAsync(
        Expression<Func<EntityChange, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<EntityChangeWithUsername> GetEntityChangeWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default);

    Task<List<EntityChangeWithUsername>> GetEntityChangesWithUsernameAsync(
        string entityId,
        string entityTypeFullName,
        CancellationToken cancellationToken = default);


    Task<List<string>> GetAllApplicationNamesAsync(CancellationToken cancellationToken = default);

    Task<List<string>> GetAllUserNamesAsync(CancellationToken cancellationToken = default);

    Task<List<string>> GetAllUrlsAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllClientIdsAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllCorrelationIdsAsync(CancellationToken cancellationToken = default);

    Task<Expression<Func<AuditLog, bool>>> BuildPredicateAsync(
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
    );

    Task<Expression<Func<EntityChange, bool>>> BuildPredicateAsync(
        string? filter,
        Guid? auditLogId,
        DateTime? startTime,
        DateTime? endTime,
        EntityChangeType? changeType,
        string? entityId
    );
}