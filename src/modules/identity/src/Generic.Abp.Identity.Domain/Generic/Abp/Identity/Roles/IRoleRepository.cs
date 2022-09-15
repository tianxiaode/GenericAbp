using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

public interface IRoleRepository: IIdentityRoleRepository
{
    Task<List<IdentityRole>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        Expression<Func<IdentityRole, bool>> predicate = null,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        string filter = null,
        Expression<Func<IdentityRole, bool>> predicate = null,
        CancellationToken cancellationToken = default);
}