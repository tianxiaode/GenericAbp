using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.SecurityLogs;

public interface ISecurityLogRepository : IRepository<IdentitySecurityLog, Guid>
{
    Task<long> GetCountAsync(Expression<Func<IdentitySecurityLog, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<IdentitySecurityLog>> GetListAsync(
        Expression<Func<IdentitySecurityLog, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<List<string>> GetAllApplicationNamesAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllIdentitiesAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllActionsAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllUserNamesAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllClientIdsAsync(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllCorrelationIdsAsync(CancellationToken cancellationToken = default);

    Task<Expression<Func<IdentitySecurityLog, bool>>> BuildPredicateAsync(
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
    );
}