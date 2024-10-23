using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

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
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<ScopeDto> GetAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<OpenIddictScope, ScopeDto>(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<PagedResultDto<ScopeDto>> GetListAsync(ScopeGetListInput input)
        {
            var sorting = input.Sorting;
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = $"{nameof(OpenIddictScope.Name)}";
            }

            var count = await Repository.GetCountAsync(input.Filter);
            var list = await Repository.GetListAsync(sorting, input.SkipCount, input.MaxResultCount, input.Filter);
            return new PagedResultDto<ScopeDto>(count,
                ObjectMapper.Map<List<OpenIddictScope>, List<ScopeDto>>(list));
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Default)]
        public virtual async Task<ListResultDto<ScopeDto>> GetAllAsync()
        {
            var list = await Repository.GetListAsync();
            return new ListResultDto<ScopeDto>(
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
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            await Repository.DeleteAsync(entity);
        }
    }
}