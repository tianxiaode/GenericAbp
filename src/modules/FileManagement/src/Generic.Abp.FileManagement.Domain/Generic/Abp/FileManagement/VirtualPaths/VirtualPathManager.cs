using Generic.Abp.Extensions.Entities;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathManager(
    IVirtualPathRepository repository,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider)
    : EntityManagerBase<VirtualPath, IVirtualPathRepository, FileManagementResource, VirtualPathSearchParams>(
        repository, localizer,
        cancellationTokenProvider)
{
    public virtual async Task<VirtualPath> FinByNameAsync(string name, bool includeDetails = true)
    {
        var entity = await FindAsync(m => m.Name.ToLower() == name.ToLower(), includeDetails);
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["VirtualPath"], name);
        }

        return entity;
    }

    public virtual Task CheckIsAccessibleAsync(VirtualPath entity)
    {
        if (!entity.IsAccessible || entity.Resource == null)
        {
            throw new EntityNotFoundBusinessException(L["VirtualPath"], entity.Name);
        }

        return Task.CompletedTask;
    }

    public override async Task ValidateAsync(VirtualPath entity)
    {
        if (await Repository.AnyAsync(m => m.Name.ToLower() == entity.Name.ToLower() && m.Id != entity.Id,
                CancellationToken))
        {
            throw new DuplicateWarningBusinessException(L["VirtualPath"], entity.Name);
        }
    }
}