using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

public partial class ResourceRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : TreeRepository<IFileManagementDbContext, Resource>(
            dbContextProvider),
        IResourceRepository
{
    public virtual async Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => code.StartsWith(m.Code) && m.HasPermissions) // 匹配所有父级和当前资源
            .OrderByDescending(r => r.Code) // 从最近到最远排序
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<long> SumSizeByCodeAsync(string code)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(m => m.Code.StartsWith(code + '.') && m.Type == ResourceType.File)
            .SumAsync(m => m.FileSize) ?? 0;
    }


    protected override async Task<IQueryable<Resource>> IncludeDetailsAsync(IIncludeOptions? option)
    {
        var resourceOptions = option as ResourceIncludeOptions ?? ResourceIncludeOptions.Default;
        return (await base.IncludeDetailsAsync(option))
            .IncludeIf(resourceOptions.IncludeParent, m => m.Parent)
            .IncludeIf(resourceOptions.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(resourceOptions.IncludePermissions, m => m.Permissions);
    }
}