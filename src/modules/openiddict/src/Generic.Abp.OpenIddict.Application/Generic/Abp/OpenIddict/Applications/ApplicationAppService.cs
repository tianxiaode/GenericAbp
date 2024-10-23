using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.OpenIddict.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Entities;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

namespace Generic.Abp.OpenIddict.Applications
{
    [RemoteService(false)]
    public class ApplicationAppService : OpenIddictAppService, IApplicationAppService
    {
        public ApplicationAppService(IOpenIddictApplicationRepository openIddictApplicationRepository,
            IOpenIddictScopeRepository scopeRepository, IAbpApplicationManager applicationManager)
        {
            Repository = openIddictApplicationRepository;
            ScopeRepository = scopeRepository;
            ApplicationManager = applicationManager;
        }

        protected IOpenIddictApplicationRepository Repository { get; }
        protected IOpenIddictScopeRepository ScopeRepository { get; }
        protected IAbpApplicationManager ApplicationManager { get; }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<ApplicationDto> GetAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(
                entity);
        }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input)
        {
            var sorting = input.Sorting;
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = $"{nameof(OpenIddictApplication.ClientId)}";
            }

            var count = await Repository.GetCountAsync(input.Filter);
            var list = await Repository.GetListAsync(sorting, input.SkipCount, input.MaxResultCount, input.Filter);
            return new PagedResultDto<ApplicationDto>(count,
                ObjectMapper.Map<List<OpenIddictApplication>, List<ApplicationDto>>(list));
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Create)]
        public virtual async Task<ApplicationDto> CreateAsync(ApplicationCreateInput input)
        {
            var entity = new OpenIddictApplication(GuidGenerator.Create());
            await UpdateByInputAsync(entity, input);

            await Repository.InsertAsync(entity);

            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input)
        {
            var entity = await Repository.GetAsync(id);
            entity.ConcurrencyStamp = input.ConcurrencyStamp;
            await UpdateByInputAsync(entity, input);
            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);
        }

        [UnitOfWork]
        protected virtual async Task UpdateByInputAsync(OpenIddictApplication entity,
            ApplicationCreateOrUpdateInput input)
        {
            var exits = await Repository.FindByClientIdAsync(input.ClientId);
            if (exits != null && (entity.Id != exits.Id))
            {
                throw new DuplicateWarningBusinessException(nameof(OpenIddictScope.Name), input.ClientId);
            }

            if (!OpenIddictConstants.ApplicationTypes.Native.Equals(input.ApplicationType) &&
                !OpenIddictConstants.ApplicationTypes.Web.Equals(input.ApplicationType))
            {
                throw new ClientTypeErrorBusinessException();
            }

            if (!OpenIddictConstants.ClientTypes.Confidential.Equals(input.ClientType) &&
                !OpenIddictConstants.ClientTypes.Public.Equals(input.ClientType))
            {
                throw new ClientTypeErrorBusinessException();
            }

            if (!OpenIddictConstants.ConsentTypes.Implicit.Equals(input.ConsentType) &&
                !OpenIddictConstants.ConsentTypes.Systematic.Equals(input.ConsentType)
                && !OpenIddictConstants.ConsentTypes.External.Equals(input.ConsentType) &&
                !OpenIddictConstants.ConsentTypes.Explicit.Equals(input.ConsentType))
            {
                throw new ConsentTypeErrorBusinessException();
            }

            entity.ClientId = input.ClientId;
            entity.DisplayName = input.DisplayName;
            entity.ClientSecret = input.ClientSecret;
            entity.ConsentType = input.ConsentType;
            entity.ClientUri = input.ClientUri;
            entity.LogoUri = input.LogoUri;
            entity.ClientType = input.ClientType;
            entity.ApplicationType = input.ApplicationType;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            await Repository.DeleteAsync(entity);
        }
    }
}