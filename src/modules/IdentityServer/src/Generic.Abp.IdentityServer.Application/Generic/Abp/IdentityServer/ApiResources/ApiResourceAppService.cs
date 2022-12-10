using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.IdentityServer.Exceptions;
using Generic.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.Uow;

namespace Generic.Abp.IdentityServer.ApiResources;

[RemoteService(false)]
public class ApiResourceAppService: IdentityServerAppService, IApiResourceAppService
{
    public ApiResourceAppService(IApiResourceRepository repository, IApiScopeRepository apiScopeRepository)
    {
        Repository = repository;
        ApiScopeRepository = apiScopeRepository;
    }

    protected IApiResourceRepository Repository { get; }
    protected IApiScopeRepository ApiScopeRepository { get; }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ApiResourceDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<ApiResource, ApiResourceDto>(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<PagedResultDto<ApiResourceDto>> GetListAsync(ApiResourceGetListDto input)
    {
        var sorting = input.Sorting;
        if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(ApiResource.Name)}";
        var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,sorting);
        var count = await Repository.GetCountAsync();
        return new PagedResultDto<ApiResourceDto>(count,
            ObjectMapper.Map<List<ApiResource>, List<ApiResourceDto>>(list));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Create)]
    public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateInput input)
    {
        var entity = new ApiResource(GuidGenerator.Create(), input.Name, input.DisplayName, input.Description) 
        {
            Enabled = input.Enabled,
            AllowedAccessTokenSigningAlgorithms = input.AllowedAccessTokenSigningAlgorithms,
            ShowInDiscoveryDocument = input.ShowInDiscoveryDocument
        };

        if (await Repository.CheckNameExistAsync(input.Name))
        {
            throw new DuplicateWarningBusinessException(nameof(ApiResource.Name), input.Name);
        }

        await Repository.InsertAsync(entity);

        var scope  = await ApiScopeRepository.FindByNameAsync(entity.Name);
        if(scope == null)
        {
            scope = new ApiScope(GuidGenerator.Create(), entity.Name);
            await ApiScopeRepository.InsertAsync(scope);
        }

        return ObjectMapper.Map<ApiResource, ApiResourceDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task<ApiResourceDto> UpdateAsync(Guid id,ApiResourceUpdateInput input)
    {
        var entity = await Repository.GetAsync(id);
        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;
        entity.Enabled = input.Enabled;
        entity.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;
        entity.AllowedAccessTokenSigningAlgorithms = input.AllowedAccessTokenSigningAlgorithms;
        entity.ConcurrencyStamp = input.ConcurrencyStamp;
        await Repository.UpdateAsync(entity);
        return ObjectMapper.Map<ApiResource, ApiResourceDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Delete)]
    public virtual async Task<ListResultDto<ApiResourceDto>> DeleteAsync(List<Guid> ids)
    {
        var result = new List<ApiResourceDto>();
        foreach (var guid in ids)
        {
            var entity = await Repository.FindAsync(guid);
            if(entity == null) continue;
            entity.RemoveAllUserClaims();
            entity.RemoveAllScopes();
            entity.RemoveAllProperties();
            entity.RemoveAllSecrets();
            await Repository.DeleteAsync(entity);
            result.Add(ObjectMapper.Map<ApiResource, ApiResourceDto>(entity));
        }

        

        return new ListResultDto<ApiResourceDto>(result);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task UpdateEnableAsync(Guid id, bool enable)
    {
        var entity = await Repository.GetAsync(id);
        entity.Enabled = enable;
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task UpdateShowInDiscoveryDocumentAsync(Guid id, bool isShow)
    {
        var entity = await Repository.GetAsync(id);
        entity.ShowInDiscoveryDocument = isShow;
        await Repository.UpdateAsync(entity);
    }

    #region Claims


    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ListResultDto<ApiResourceClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ApiResourceClaimDto>(
            ObjectMapper.Map<List<ApiResourceClaim>, List<ApiResourceClaimDto>>(entity.UserClaims));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task AddClaimAsync(Guid id, ApiResourceClaimCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.UserClaims.Any(m=>m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddUserClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task RemoveClaimAsync(Guid id, ApiResourceClaimDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(!entity.UserClaims.Any(m=>m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveClaim(input.Type);
        await Repository.UpdateAsync(entity);
    }

    #endregion

    #region Scopes

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ListResultDto<ApiResourceScopeDto>> GetScopesAsync(Guid id)
    {
        //var scopes = await ApiScopeRepository.GetListAsync();
        //var list = scopes.Select(m => new ApiResourceScopeDto(m.Name, m.DisplayName)).ToList();
        var entity = await Repository.GetAsync(id);
        var list = entity.Scopes.Select(m=>new ApiResourceScopeDto(m.Scope)).ToList();

        return new ListResultDto<ApiResourceScopeDto>(list);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task AddScopeAsync(Guid id, ApiResourceScopeCreateInput input)
    {
        var scopeName = input.Name;
        var scope = await ApiScopeRepository.FindByNameAsync(scopeName);
        if (scope == null) throw new EntityNotFoundBusinessException(nameof(ApiScope), scopeName);
        var entity = await Repository.GetAsync(id);
        entity.AddScope(scopeName);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task RemoveScopeAsync(Guid id, ApiResourceScopeDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.Name.Equals(input.Name, StringComparison.CurrentCultureIgnoreCase)) throw new RemovingTheDefaultScopeIsNotAllowedBusinessException();
        entity.RemoveScope(input.Name);
        await Repository.UpdateAsync(entity);
    }
    
    #endregion

    #region secrets

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Default)]
    public virtual async Task<ListResultDto<ApiResourceSecretDto>> GetClientSecretsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ApiResourceSecretDto>(
            ObjectMapper.Map<List<ApiResourceSecret>, List<ApiResourceSecretDto>>(entity.Secrets));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task AddSecretAsync(Guid id, ApiResourceSecretCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.Secrets.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddSecret(input.Value, input.Expiration, input.Type, input.Description);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.ApiResources.Update)]
    public virtual async Task RemoveSecretAsync(Guid id, ApiResourceSecretDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.Secrets.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveSecret(input.Value, input.Type);
        await Repository.UpdateAsync(entity);
    }


    #endregion
}