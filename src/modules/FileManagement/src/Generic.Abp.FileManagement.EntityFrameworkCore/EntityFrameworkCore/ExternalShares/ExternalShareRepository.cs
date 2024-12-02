using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.Extensions.EntityFrameworkCore;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.ExternalShares;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.ExternalShares;

public class ExternalShareRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : ExtensionRepository<IFileManagementDbContext, ExternalShare, ExternalShareSearchParams>(dbContextProvider),
        IExternalShareRepository
{
    public override Task<Expression<Func<ExternalShare, bool>>> BuildPredicateExpression(
        ExternalShareSearchParams searchParams)
    {
        Expression<Func<ExternalShare, bool>> predicate = x => true;
        if (!string.IsNullOrEmpty(searchParams.Filter))
        {
            predicate = predicate.AndIfNotTrue(x => x.LinkName.Contains(searchParams.Filter));
        }

        if (searchParams.StartTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(x => x.CreationTime >= searchParams.StartTime.Value);
        }

        if (searchParams.EndTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(x => x.CreationTime <= searchParams.EndTime.Value);
        }

        if (searchParams.ExpireTimeStart.HasValue)
        {
            predicate = predicate.AndIfNotTrue(x => x.ExpireTime >= searchParams.ExpireTimeStart.Value);
        }

        if (searchParams.ExpireTimeEnd.HasValue)
        {
            predicate = predicate.AndIfNotTrue(x => x.ExpireTime <= searchParams.ExpireTimeEnd.Value);
        }

        if (searchParams.OwnerId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(x => x.CreatorId == searchParams.OwnerId.Value);
        }

        return Task.FromResult(predicate);
    }
}