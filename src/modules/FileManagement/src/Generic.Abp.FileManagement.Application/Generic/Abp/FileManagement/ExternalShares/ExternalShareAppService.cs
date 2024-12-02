using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Settings;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareAppService(
    ExternalShareManager externalShareManager,
    FileManagementSettingManager settingManager)
    : FileManagementAppService, IExternalShareAppService, IExternalShareAppService1
{
    protected ExternalShareManager ExternalShareManager { get; } = externalShareManager;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;

    [Authorize]
    public virtual async Task<ExternalShareDto> GetMyExternalShareAsync(Guid id)
    {
        var entity = await ExternalShareManager.GetAsync(id);
        if (entity.CreatorId != CurrentUser.Id)
        {
            throw new EntityNotFoundBusinessException(L["ExternalShare"], id);
        }

        return ObjectMapper.Map<ExternalShare, ExternalShareDto>(entity);
    }

    [Authorize]
    public virtual async Task<PagedResultDto<ExternalShareDto>> GetMyExternalSharesAsync(
        ExternalShareGetListInput input)
    {
        input.OwnerId = CurrentUser.Id;
        return await GetListBaseAsync(input);
    }

    [Authorize]
    public virtual async Task DeleteMyExternalSharesAsync(List<Guid> ids)
    {
        var list = await ExternalShareManager.GetListAsync(m => m.CreatorId == CurrentUser.Id && ids.Contains(m.Id),
            ExternalShareConsts.GetDefaultSorting());
        await ExternalShareManager.DeleteManyAsync(list);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Default)]
    public virtual async Task<ExternalShareDto> GetAsync(Guid id)
    {
        var entity = await ExternalShareManager.GetAsync(id);
        return ObjectMapper.Map<ExternalShare, ExternalShareDto>(entity);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Default)]
    public virtual async Task<PagedResultDto<ExternalShareDto>> GetListAsync(ExternalShareGetListInput input)
    {
        return await GetListBaseAsync(input);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Create)]
    public virtual async Task<ExternalShareDto> CreateAsync(ExternalShareCreateDto input)
    {
        var entity = new ExternalShare(GuidGenerator.Create(), input.ResourceId,
            await SettingManager.GetExpirationDateOfExternalSharedAsync(),
            CurrentTenant.Id);
        await ExternalShareManager.CreateAsync(entity);
        return ObjectMapper.Map<ExternalShare, ExternalShareDto>(entity);
    }

    [Authorize(FileManagementPermissions.ExternalShares.Delete)]
    public virtual async Task DeleteManyAsync(List<Guid> ids)
    {
        await ExternalShareManager.DeleteManyAsync(ids);
    }

    protected virtual async Task<PagedResultDto<ExternalShareDto>> GetListBaseAsync(ExternalShareGetListInput input)
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
}