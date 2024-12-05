using Generic.Abp.Extensions.Entities;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Tokens;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareManager(
    IExternalShareRepository repository,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider,
    TokenManager tokenManager,
    ResourceManager resourceManager)
    : EntityManagerBase<ExternalShare, IExternalShareRepository, FileManagementResource>(
        repository, localizer,
        cancellationTokenProvider)
{
    protected TokenManager TokenManager { get; } = tokenManager;
    protected ResourceManager ResourceManager { get; } = resourceManager;

    public virtual async Task<ExternalShare> FindByLinkNameAsync(string linkName)
    {
        var entity = await Repository.FindAsync(m => string.Equals(m.LinkName, linkName.ToLowerInvariant()), false,
            CancellationToken).ConfigureAwait(false);
        ;
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["ExternalShare"], linkName);
        }

        return entity;
    }

    public virtual Task CanRedAsync(ExternalShare entity, string password)
    {
        if (entity.Password != password)
        {
            throw new AbpAuthorizationException(L["InvalidPassword"]);
        }

        var now = DateTime.UtcNow;

        if (now > entity.ExpireTime)
        {
            throw new AbpAuthorizationException(L["ExternalShareExpired"]);
        }

        return Task.CompletedTask;
    }

    public virtual async Task<(long, List<Resource>)> GetResourcesAsync(ExternalShare entity, Guid? resourceId,
        ResourceQueryParams query)
    {
        var root = await Repository.GetAsync(entity.ResourceId).ConfigureAwait(false);
        if (root == null)
        {
            throw new EntityNotFoundBusinessException(L["Resource"], entity.LinkName);
        }

        query.ParentId = root.Id;
        if (resourceId.HasValue)
        {
            var isExist = await ResourceManager.IsExistsAsync(resourceId.Value, entity.ResourceId)
                .ConfigureAwait(false);
            if (!isExist)
            {
                throw new EntityNotFoundBusinessException(L["Resource"], entity.LinkName);
            }

            query.ParentId = resourceId.Value;
        }

        var predicate = await ResourceManager.BuildPredicateExpressionAsync(query).ConfigureAwait(false);

        var count = await ResourceManager.GetCountAsync(predicate).ConfigureAwait(false);
        var list = await ResourceManager.GetPagedListAsync(predicate, query).ConfigureAwait(false);
        return (count, list);
    }

    public virtual async Task<string> GetTokenAsync(ExternalShare entity)
    {
        var tokenData = TokenManager.GenerateTokenData(entity.Id, entity.ExpireTime, "");
        entity.SetToken(tokenData);
        await Repository.UpdateAsync(entity, true, CancellationToken).ConfigureAwait(false);
        return tokenData.Token;
    }

    public virtual Task ValidateTokenAsync(ExternalShare entity, string token)
    {
        var tokenData = entity.GetToken<ExternalShare, object>(token);
        if (tokenData == null)
        {
            throw new AbpAuthorizationException(L["InvalidToken"]);
        }

        if (tokenData.ExpireTime < DateTime.UtcNow)
        {
            throw new AbpAuthorizationException(L["TokenExpired"]);
        }

        var (isValid, data) = TokenManager.ValidateTokenData<string>(token, tokenData.Key);
        if (!isValid)
        {
            throw new AbpAuthorizationException(L["InvalidToken"]);
        }

        return Task.CompletedTask;
    }
}