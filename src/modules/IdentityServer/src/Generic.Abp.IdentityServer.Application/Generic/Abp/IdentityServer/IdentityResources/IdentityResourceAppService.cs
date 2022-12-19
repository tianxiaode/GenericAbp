using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Uow;

namespace Generic.Abp.IdentityServer.IdentityResources;

[RemoteService(false)]
public class IdentityResourceAppService : IdentityServerAppService, IIdentityResourceAppService
{
    public IdentityResourceAppService(IIdentityResourceRepository repository)
    {
        Repository = repository;
    }

    protected IIdentityResourceRepository Repository { get; }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Default)]
    public virtual async Task<IdentityResourceDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Default)]
    public virtual async Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetListDto input)
    {
        var sorting = input.Sorting;
        if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(IdentityResource.Name)}";
        var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
        var count = await Repository.GetCountAsync();
        return new PagedResultDto<IdentityResourceDto>(count,
            ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceDto>>(list));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Create)]
    public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateInput input)
    {
        var entity = new IdentityResource(GuidGenerator.Create(), input.Name, input.DisplayName, input.Description,
            input.Enabled, input.Required, input.Emphasize, input.ShowInDiscoveryDocument);
        if (await Repository.CheckNameExistAsync(input.Name))
        {
            throw new DuplicateWarningBusinessException(nameof(IdentityResource.Name), input.Name);
        }

        await Repository.InsertAsync(entity);
        return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceUpdateInput input)
    {
        var entity = await Repository.GetAsync(id, false);
        if (await Repository.CheckNameExistAsync(input.Name, entity.Id))
        {
            throw new DuplicateWarningBusinessException(nameof(IdentityResource.Name), input.Name);
        }
        entity.Name = input.Name;
        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;
        entity.Enabled = input.Enabled;
        entity.Required = input.Required;
        entity.Emphasize = input.Emphasize;
        entity.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;
        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        await Repository.UpdateAsync(entity);
        return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Delete)]
    public virtual async Task<ListResultDto<IdentityResourceDto>> DeleteAsync(List<Guid> ids)
    {
        var result = new List<IdentityResourceDto>();
        foreach (var guid in ids)
        {
            var entity = await Repository.FindAsync(guid);
            if (entity == null) continue;
            await Repository.DeleteAsync(entity);
            result.Add(ObjectMapper.Map<IdentityResource, IdentityResourceDto>(entity));
        }

        return new ListResultDto<IdentityResourceDto>(result);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task UpdateEnableAsync(Guid id, bool enable)
    {
        var entity = await Repository.GetAsync(id);
        entity.Enabled = enable;
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task UpdateShowInDiscoveryDocumentAsync(Guid id, bool isShow)
    {
        var entity = await Repository.GetAsync(id);
        entity.ShowInDiscoveryDocument = isShow;
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task UpdateEmphasizeAsync(Guid id, bool isEmphasize)
    {
        var entity = await Repository.GetAsync(id);
        entity.Emphasize = isEmphasize;
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task UpdateRequiredAsync(Guid id, bool isRequire)
    {
        var entity = await Repository.GetAsync(id);
        entity.Required = isRequire;
        await Repository.UpdateAsync(entity);
    }



    #region claims
    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Default)]
    public virtual async Task<ListResultDto<IdentityResourceClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<IdentityResourceClaimDto>(
            ObjectMapper.Map<List<IdentityResourceClaim>, List<IdentityResourceClaimDto>>(entity.UserClaims));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task AddClaimAsync(Guid id, IdentityResourceClaimCrateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.UserClaims.Any(m => m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddUserClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.IdentityResources.Update)]
    public virtual async Task RemoveClaimAsync(Guid id, IdentityResourceClaimDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.UserClaims.Any(m => m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveUserClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }
    #endregion

    #region Properties
    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ListResultDto<IdentityResourcePropertyDto>> GetPropertiesAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<IdentityResourcePropertyDto>(
            ObjectMapper.Map<List<IdentityResourceProperty>, List<IdentityResourcePropertyDto>>(entity.Properties));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task AddPropertyAsync(Guid id, IdentityResourcePropertyCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.Properties.Any(m => m.Key.Equals(input.Key, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new DuplicateWarningBusinessException(nameof(IdentityResourceProperty), input.Key);
        };
        entity.AddProperty(input.Key, input.Value);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task RemovePropertyAsync(Guid id, IdentityResourcePropertyDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.Properties.Any(m => m.Key.Equals(input.Key, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveProperty(input.Key);
        await Repository.UpdateAsync(entity);
    }
    #endregion
}