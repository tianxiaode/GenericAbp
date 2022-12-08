using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.IdentityServer.ApiResources;
using Generic.Abp.IdentityServer.Exceptions;
using Generic.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Uow;

namespace Generic.Abp.IdentityServer.ApiScopes;

[RemoteService(false)]
public class ApiScopeAppService: IdentityServerAppService, IApiScopeAppService
{
    public ApiScopeAppService(IApiScopeRepository repository, IApiResourceRepository apiResourceRepository)
    {
        Repository = repository;
        ApiResourceRepository = apiResourceRepository;
    }

    protected IApiScopeRepository Repository { get; }
    protected IApiResourceRepository ApiResourceRepository { get; }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ApiScopeDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<ApiScope, ApiScopeDto>(entity);
    }



    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<PagedResultDto<ApiScopeDto>> GetListAsync()
    {
        var sorting = $"{nameof(ApiScope.Name)}";
        var list = await Repository.GetPagedListAsync(0, 1000,sorting);
        var count = await Repository.GetCountAsync();
        return new PagedResultDto<ApiScopeDto>(count,
            ObjectMapper.Map<List<ApiScope>, List<ApiScopeDto>>(list));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task<ApiScopeDto> CreateAsync(ApiScopeCreateInput input)
    {
        var entity = new ApiScope(GuidGenerator.Create(), input.Name, input.DisplayName, input.Description) 
        {
            Enabled = input.Enabled,
            Emphasize = input.Emphasize,
            Required = input.Required,
            ShowInDiscoveryDocument = input.ShowInDiscoveryDocument
        };

        if (await Repository.CheckNameExistAsync(input.Name))
        {
            throw new DuplicateWarningBusinessException(nameof(IdentityResource.Name), input.Name);
        }

        await Repository.InsertAsync(entity);
        return ObjectMapper.Map<ApiScope, ApiScopeDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task<ApiScopeDto> UpdateAsync(Guid id,ApiScopeUpdateInput input)
    {
        var entity = await Repository.GetAsync(id, false);
        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;
        entity.Enabled = input.Enabled;
        entity.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;
        entity.Required = input.Required;
        entity.Emphasize = input.Emphasize;
        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        await Repository.UpdateAsync(entity);
        return ObjectMapper.Map<ApiScope, ApiScopeDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Delete)]
    public virtual async Task<ListResultDto<ApiScopeDto>> DeleteAsync(List<Guid> ids)
    {
        var result = new List<ApiScopeDto>();
        var deletes = new List<ApiScope>();
        foreach (var guid in ids)
        {
            var entity = await Repository.FindAsync(guid);
            if(entity == null) continue;
            deletes.Add(entity);
        }
        var scopeNames = deletes.Select(x => x.Name).ToArray();
        var apiResources = await ApiResourceRepository.GetListByScopesAsync(scopeNames);
        if(apiResources.Any()) throw new ApiScopesInUseBusinessException();
        foreach (var delete in deletes)
        {
            await Repository.DeleteAsync(delete);
            result.Add(ObjectMapper.Map<ApiScope, ApiScopeDto>(delete));
        }

        return new ListResultDto<ApiScopeDto>(result);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ListResultDto<ApiScopeClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ApiScopeClaimDto>(
            ObjectMapper.Map<List<ApiScopeClaim>, List<ApiScopeClaimDto>>(entity.UserClaims));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task AddClaimAsync(Guid id, ApiScopeClaimCrateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.UserClaims.Any(m=>m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddUserClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task RemoveClaimAsync(Guid id, ApiScopeClaimDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(!entity.UserClaims.Any(m=>m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }
}