using Generic.Abp.Extensions.EntityFrameworkCore;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.VirtualPaths;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;

public class VirtualPathRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : ExtensionRepository<FileManagementDbContext, VirtualPath, VirtualPathSearchParams>(dbContextProvider),
        IVirtualPathRepository
{
    public override Task<Expression<Func<VirtualPath, bool>>> BuildPredicateExpression(
        VirtualPathSearchParams searchParams)
    {
        Expression<Func<VirtualPath, bool>> predicate = (virtualPath) => true;

        if (!string.IsNullOrEmpty(searchParams.Filter))
        {
            predicate = predicate.OrIfNotTrue(m => m.Name.Contains(searchParams.Filter));
        }

        if (searchParams.StartTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= searchParams.StartTime);
        }

        if (searchParams.EndTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= searchParams.EndTime);
        }

        if (searchParams.IsAccessible.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.IsAccessible == searchParams.IsAccessible);
        }

        return Task.FromResult(predicate);
    }
}