using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Uow;

namespace Generic.Abp.IdentityServer.Clients;

[RemoteService(false)]
public class ClientAppService: IdentityServerAppService, IClientAppService
{
    public ClientAppService(IClientRepository repository)
    {
        Repository = repository;
    }

    protected IClientRepository Repository { get; }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ClientDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return ObjectMapper.Map<Client, ClientDto>(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetListDto input)
    {
        var sorting = input.Sorting;
        if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(Client.ClientId)}";
        var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
        var count = await Repository.GetCountAsync();
        return new PagedResultDto<ClientDto>(count,
            ObjectMapper.Map<List<Client>, List<ClientDto>>(list));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Create)]
    public virtual async Task<ClientDto> CreateAsync(ClientCreateInput input)
    {
        if (await Repository.CheckClientIdExistAsync(input.ClientId))
        {
            throw new DuplicateWarningBusinessException(nameof(Client.ClientId), input.ClientId);
        }
        var entity = new Client(GuidGenerator.Create(), input.ClientId);
        ObjectMapper.Map(input, entity);
        await Repository.InsertAsync(entity);
        return ObjectMapper.Map<Client, ClientDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task<ClientDto> UpdateAsync(Guid id,ClientUpdateInput input)
    {
        var entity = await Repository.GetAsync(id, false);
        if (await Repository.CheckClientIdExistAsync(input.ClientId, entity.Id))
        {
            throw new DuplicateWarningBusinessException(nameof(Client.ClientId), input.ClientId);
        }
        entity.ClientId = input.ClientId;
        entity.ConcurrencyStamp = input.ConcurrencyStamp;

        await Repository.UpdateAsync(entity);
        return ObjectMapper.Map<Client, ClientDto>(entity);

    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Delete)]
    public virtual async Task<ListResultDto<ClientDto>> DeleteAsync(List<Guid> ids)
    {
        var result = new List<ClientDto>();
        foreach (var guid in ids)
        {
            var entity = await Repository.FindAsync(guid);
            if(entity == null) continue;
            await Repository.DeleteAsync(entity);
            result.Add(ObjectMapper.Map<Client, ClientDto>(entity));
        }

        return new ListResultDto<ClientDto>(result);
    }

    #region ClientClaims

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientClaimDto>(
            ObjectMapper.Map<List<ClientClaim>, List<ClientClaimDto>>(entity.Claims));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddClaimAsync(Guid id, ClientClaimCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.Claims.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddClaim(input.Type,input.Value);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveClaimAsync(Guid id, ClientClaimDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.Claims.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveClaim(input.Type, input.Value);
        await Repository.UpdateAsync(entity);
    }
    #endregion

    #region ClientCorsOrigins

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientCorsOriginDto>> GetCorsOriginsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientCorsOriginDto>(
            ObjectMapper.Map<List<ClientCorsOrigin>, List<ClientCorsOriginDto>>(entity.AllowedCorsOrigins));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddCorsOriginAsync(Guid id, ClientCorsOriginCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.AllowedCorsOrigins.Any(m=>m.Origin.Equals(input.Origin, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddCorsOrigin(input.Origin);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveCorsOriginAsync(Guid id, ClientCorsOriginDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(!entity.AllowedCorsOrigins.Any(m=>m.Origin.Equals(input.Origin, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveCorsOrigin(input.Origin);
        await Repository.UpdateAsync(entity);
    }
    

    #endregion

    #region ClientGrantTypes

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientGrantTypeDto>> GetGrantTypesAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientGrantTypeDto>(
            ObjectMapper.Map<List<ClientGrantType>, List<ClientGrantTypeDto>>(entity.AllowedGrantTypes));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddGrantTypeAsync(Guid id, ClientGrantTypeCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.AllowedGrantTypes.Any(m=>m.GrantType.Equals(input.GrantType, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddGrantType(input.GrantType);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveGrantTypeAsync(Guid id, ClientGrantTypeDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(!entity.AllowedGrantTypes.Any(m=>m.GrantType.Equals(input.GrantType, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveGrantType(input.GrantType);
        await Repository.UpdateAsync(entity);
    }
    #endregion

    #region CLientScopes

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientScopeDto>> GetScopesAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientScopeDto>(
            ObjectMapper.Map<List<ClientScope>, List<ClientScopeDto>>(entity.AllowedScopes));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddScopeAsync(Guid id, ClientScopeCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(entity.AllowedScopes.Any(m=>m.Scope.Equals(input.Scope, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddScope(input.Scope);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveScopeAsync(Guid id, ClientScopeDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if(!entity.AllowedScopes.Any(m=>m.Scope.Equals(input.Scope, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveScope(input.Scope);
        await Repository.UpdateAsync(entity);
    }
    #endregion

    #region ClientSecrets

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientSecretDto>> GetClientSecretsAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientSecretDto>(
            ObjectMapper.Map<List<ClientSecret>, List<ClientSecretDto>>(entity.ClientSecrets));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddSecretAsync(Guid id, ClientSecretCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.ClientSecrets.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddSecret(input.Value, input.Expiration, input.Type, input.Description);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveSecretAsync(Guid id, ClientSecretDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.ClientSecrets.Any(m =>
                m.Type.Equals(input.Type, StringComparison.InvariantCultureIgnoreCase) &&
                m.Value.Equals(input.Value, StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveSecret(input.Value, input.Type);
        await Repository.UpdateAsync(entity);
    }
    

    #endregion

    #region ClientPostLogoutRedirectUri

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientPostLogoutRedirectUriDto>> GetPostLogoutRedirectUrisAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientPostLogoutRedirectUriDto>(
            ObjectMapper.Map<List<ClientPostLogoutRedirectUri>, List<ClientPostLogoutRedirectUriDto>>(entity.PostLogoutRedirectUris));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddPostLogoutRedirectUriAsync(Guid id, ClientPostLogoutRedirectUriCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.PostLogoutRedirectUris.Any(m =>
                m.PostLogoutRedirectUri.Equals(input.PostLogoutRedirectUri,
                    StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddPostLogoutRedirectUri(input.PostLogoutRedirectUri);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemovePostLogoutRedirectUriAsync(Guid id, ClientPostLogoutRedirectUriDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.PostLogoutRedirectUris.Any(m =>
                m.PostLogoutRedirectUri.Equals(input.PostLogoutRedirectUri,
                    StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemovePostLogoutRedirectUri(input.PostLogoutRedirectUri);
        await Repository.UpdateAsync(entity);
    }


    #endregion

    #region RedirectUris

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Default)]
    public virtual async Task<ListResultDto<ClientRedirectUriDto>> GetRedirectUrisAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        return new ListResultDto<ClientRedirectUriDto>(
            ObjectMapper.Map<List<ClientRedirectUri>, List<ClientRedirectUriDto>>(entity.RedirectUris));
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task AddRedirectUriAsync(Guid id, ClientRedirectUriCreateInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (entity.RedirectUris.Any(m =>
                m.RedirectUri.Equals(input.RedirectUri,
                    StringComparison.InvariantCultureIgnoreCase))) return;
        entity.AddRedirectUri(input.RedirectUri);
        await Repository.UpdateAsync(entity);
    }

    [UnitOfWork]
    [Authorize(IdentityServerPermissions.Clients.Update)]
    public virtual async Task RemoveRedirectUriAsync(Guid id, ClientRedirectUriDeleteInput input)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.RedirectUris.Any(m =>
                m.RedirectUri.Equals(input.RedirectUri,
                    StringComparison.InvariantCultureIgnoreCase))) return;
        entity.RemoveRedirectUri(input.RedirectUri);
        await Repository.UpdateAsync(entity);
    }
    

    #endregion
}