using System.Text.Json;
using System.Text.Json.Nodes;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

namespace Generic.Abp.OpenIddict.Scopes
{
    [RemoteService(false)]
    public class ScopeAppService : OpenIddictAppService, IScopeAppService
    {
        protected IOpenIddictScopeRepository Repository { get; }
        protected IOpenIddictScopeManager Manager { get; }

        public ScopeAppService(IOpenIddictScopeRepository repository, IOpenIddictScopeManager manager)
        {
            Repository = repository;
            Manager = manager;
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
            await ReplaceScopeNameAsync(input);
            await CheckDuplicateNameAsync(input.Name);
            var descriptor = new OpenIddictScopeDescriptor
            {
                Name = input.Name,
            };

            await UpdateByInputAsync(descriptor, input);

            var scope = await Manager.CreateAsync(descriptor);
            input.MapExtraPropertiesTo(scope.As<OpenIddictScopeModel>());
            await Manager.UpdateAsync(scope);

            return await GetAsync(scope.As<OpenIddictScopeModel>().Id);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Update)]
        public virtual async Task<ScopeDto> UpdateAsync(Guid id, ScopeUpdateInput input)
        {
            await ReplaceScopeNameAsync(input);
            var scope = await Manager.FindByIdAsync(id.ToString("D"));
            if (scope == null)
            {
                throw new EntityNotFoundException(typeof(OpenIddictScope), id);
            }

            var model = scope.As<OpenIddictScopeModel>();

            if (!string.Equals(model.Name, input.Name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckDuplicateNameAsync(input.Name);
            }

            var descriptor = new OpenIddictScopeDescriptor
            {
                Name = input.Name,
            };
            await Manager.PopulateAsync(descriptor, scope);
            await UpdateByInputAsync(descriptor, input);

            input.MapExtraPropertiesTo(scope.As<OpenIddictScopeModel>());
            await Manager.UpdateAsync(scope, descriptor);
            return await GetAsync(id);
        }

        [UnitOfWork]
        protected virtual Task UpdateByInputAsync(OpenIddictScopeDescriptor scope, ScopeCreateOrUpdateInput input)
        {
            scope.Name = input.Name;
            scope.DisplayName = input.DisplayName;
            scope.Description = input.Description;
            // scope.Properties.Clear();
            // foreach (var property in input.Properties)
            // {
            //     scope.Properties.Add(property.Key, JsonSerializer.SerializeToElement(property.Value));
            // }

            scope.Resources.Clear();
            scope.Resources.UnionWith(input.Resources);
            foreach (var resource in input.Resources)
            {
                scope.Resources.Add(resource);
            }

            return Task.CompletedTask;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Scopes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var scope = await Manager.FindByIdAsync(id.ToString("D"));
            if (scope == null)
            {
                throw new EntityNotFoundException(typeof(OpenIddictScope), id);
            }

            await Manager.DeleteAsync(scope);
        }

        protected virtual async Task CheckDuplicateNameAsync(string name)
        {
            var exits = await Repository.FindByNameAsync(name);
            if (exits != null)
            {
                throw new DuplicateWarningBusinessException(nameof(OpenIddictScope.Name), name);
            }
        }

        protected virtual Task ReplaceScopeNameAsync(ScopeCreateOrUpdateInput input)
        {
            //将name中的空格替换为下划线
            input.Name = input.Name.Replace(" ", "_");
            return Task.CompletedTask;
        }
    }
}