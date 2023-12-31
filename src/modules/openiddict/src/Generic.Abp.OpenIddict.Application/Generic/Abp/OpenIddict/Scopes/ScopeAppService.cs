using Generic.Abp.BusinessException.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;
using Generic.Abp.OpenIddict.Permissions;
using Volo.Abp;
using Generic.Abp.OpenIddict.Applications;
using System.Linq;
using Volo.Abp.Domain.ChangeTracking;

namespace Generic.Abp.OpenIddict.Scopes
{
    [RemoteService(false)]
    public class ScopeAppService : OpenIddictAppService, IScopeAppService
    {
        protected IOpenIddictScopeRepository Repository { get; }

        public ScopeAppService(IOpenIddictScopeRepository repository)
        {
            Repository = repository;
        }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<ScopeDto> GetAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<OpenIddictScope, ScopeDto>(entity);
        }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<PagedResultDto<ScopeDto>> GetListAsync(ScopeGetListInput input)
        {
            var sorting = input.Sorting;
            if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(OpenIddictScope.Name)}";
            var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
            var count = await Repository.GetCountAsync();
            return new PagedResultDto<ScopeDto>(count,
                ObjectMapper.Map<List<OpenIddictScope>, List<ScopeDto>>(list));
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Create)]
        public virtual async Task<ScopeDto> CreateAsync(ScopeCreateInput input)
        {
            var entity = new OpenIddictScope(GuidGenerator.Create());
            await UpdateByInputAsync(entity, input);

            await Repository.InsertAsync(entity);

            return ObjectMapper.Map<OpenIddictScope, ScopeDto>(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task<ScopeDto> UpdateAsync(Guid id, ScopeUpdateInput input)
        {
            var entity = await Repository.GetAsync(id);
            entity.ConcurrencyStamp = input.ConcurrencyStamp;
            await UpdateByInputAsync(entity, input);
            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<OpenIddictScope, ScopeDto>(entity);
        }

        [UnitOfWork]
        protected virtual async Task UpdateByInputAsync(OpenIddictScope entity, ScopeCreateOrUpdateInput input)
        {
            var exits = await Repository.FindByNameAsync(input.Name);
            if (exits != null && (entity.Id != exits.Id))
            {
                throw new DuplicateWarningBusinessException(nameof(OpenIddictScope.Name), input.Name);
            }

            entity.DisplayName = input.DisplayName;
            entity.Description = input.Description;
            entity.Name = input.Name;
            if (!input.Properties.IsNullOrEmpty())
            {
                entity.Properties = System.Text.Json.JsonSerializer.Serialize(input.Properties);
            }

            if (!input.Resources.IsNullOrEmpty())
            {
                entity.Resources = System.Text.Json.JsonSerializer.Serialize(input.Resources);
            }
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Delete)]
        public virtual async Task<ListResultDto<ScopeDto>> DeleteAsync(List<Guid> ids)
        {
            var result = new List<ScopeDto>();
            foreach (var guid in ids)
            {
                var entity = await Repository.FindAsync(guid);
                if (entity == null) continue;
                await Repository.DeleteAsync(entity);
                result.Add(ObjectMapper.Map<OpenIddictScope, ScopeDto>(entity));
            }


            return new ListResultDto<ScopeDto>(result);
        }

        #region Properties

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<List<string>> GetPropertiesAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Properties.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Properties,
                    new System.Text.Json.JsonSerializerOptions());
            return list;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task AddPropertyAsync(Guid id, ScopePropertyCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Properties.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Properties,
                    new System.Text.Json.JsonSerializerOptions());
            if (list.Any(m => m.Equals(input.Value))) return;
            list.Add(input.Value);
            entity.Properties = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task RemovePropertyAsync(Guid id, ScopePropertyDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Properties.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Properties,
                    new System.Text.Json.JsonSerializerOptions());
            foreach (var item in input.Items)
            {
                list.Remove(item);
            }

            entity.Properties = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        #endregion

        #region Resources

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<List<string>> GetResourcesAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Resources.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Resources,
                    new System.Text.Json.JsonSerializerOptions());
            return list;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task AddResourceAsync(Guid id, ScopeResourceCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Resources.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Resources,
                    new System.Text.Json.JsonSerializerOptions());
            if (list.Any(m => m.Equals(input.Value))) return;
            list.Add(input.Value);
            entity.Resources = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task RemoveResourceAsync(Guid id, ScopeResourceDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Resources.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Resources,
                    new System.Text.Json.JsonSerializerOptions());
            foreach (var item in input.Items)
            {
                list.Remove(item);
            }

            entity.Resources = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        #endregion
    }
}