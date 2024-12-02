using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
using Generic.Abp.FileManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareAppService : FileManagementAppService, IExternalShareAppService
{
    public ExternalShareAppService(ExternalShareManager externalShareManager)
    {
        ExternalShareManager = externalShareManager;
    }

    protected ExternalShareManager ExternalShareManager { get; }

    [Authorize(FileManagementPermissions.ExternalShares.Default)]
    public virtual async Task<ExternalShareDto> GetAsync(Guid id)
    {
        var entity = await ExternalShareManager.GetAsync(id);
        return ObjectMapper.Map<ExternalShare, ExternalShareDto>(entity);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Default)]
    public virtual async Task<PagedResultDto<ExternalShareDto>> GetListAsync(ExternalShareGetListInput input)
    {
        var predicate =
            await ExternalShareManager.BuildPredicateExpression(
                ObjectMapper.Map<ExternalShareGetListInput, ExternalShareSearchParams>(input));
        var count = await ExternalShareManager.GetCountAsync(predicate);
        var list = await ExternalShareManager.GetListAsync(predicate,
            input.Sorting ?? ExternalShareConsts.GetDefaultSorting(), input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ExternalShareDto>(count,
            ObjectMapper.Map<List<ExternalShare>, List<ExternalShareDto>>(list));
    }

    [Authorize(FileManagementPermissions.ExternalShares.Create)]
    public virtual async Task<ExternalShareDto> CreateAsync(ExternalShareCreateDto input)
    {
        var entity = await ExternalShareManager.CreateAsync(new ExternalShare(GuidGenerator.Create(), input.ResourceId, , CurrentTenant.Id));
        return ObjectMapper.Map<ExternalShare, ExternalShareDto>(entity);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await ExternalShareManager.DeleteAsync(id);
    }
}