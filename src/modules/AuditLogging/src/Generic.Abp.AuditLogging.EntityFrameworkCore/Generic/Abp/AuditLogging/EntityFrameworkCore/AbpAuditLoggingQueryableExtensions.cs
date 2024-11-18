using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;

namespace Generic.Abp.AuditLogging.EntityFrameworkCore;

public static class AbpAuditLoggingQueryableExtensions
{
    public static IQueryable<AuditLog> IncludeDetails(
        this IQueryable<AuditLog> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Actions)
            .Include(x => x.EntityChanges).ThenInclude(ec => ec.PropertyChanges);
    }

    public static IQueryable<EntityChange> IncludeDetails(
        this IQueryable<EntityChange> queryable,
        bool include = true)
    {
        return !include ? queryable : queryable.Include(x => x.PropertyChanges);
    }
}