using System.Text.Json;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.OpenIddict.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Generic.Abp.OpenIddict.Scopes;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;
using static System.Formats.Asn1.AsnWriter;

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
            await CheckInputAsync(input);
            await CheckDuplicateClientIdAsync(input.ClientId);
            var descriptor = new AbpApplicationDescriptor
            {
                ClientId = input.ClientId
            };

            await UpdateByInputAsync(descriptor, input);

            var scope = await ApplicationManager.CreateAsync(descriptor);
            input.MapExtraPropertiesTo(scope.As<OpenIddictApplicationModel>());
            await ApplicationManager.UpdateAsync(scope);

            return await GetAsync(scope.As<OpenIddictApplicationModel>().Id);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input)
        {
            await CheckInputAsync(input);
            var application = await ApplicationManager.FindByIdAsync(id.ToString("D"));
            if (application == null)
            {
                throw new EntityNotFoundException(typeof(OpenIddictApplication), id);
            }

            var model = application.As<OpenIddictApplicationModel>();
            if (!string.Equals(model.ClientId, input.ClientId, StringComparison.OrdinalIgnoreCase))
            {
                await CheckDuplicateClientIdAsync(input.ClientId);
            }

            var descriptor = new AbpApplicationDescriptor
            {
                ClientId = input.ClientId,
            };

            await ApplicationManager.PopulateAsync(descriptor, application);
            await UpdateByInputAsync(descriptor, input);

            input.MapExtraPropertiesTo(ApplicationManager.As<OpenIddictApplicationModel>());
            await ApplicationManager.UpdateAsync(application, descriptor);
            var entity = await Repository.GetAsync(id);
            entity.ConcurrencyStamp = input.ConcurrencyStamp;
            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);
        }

        [UnitOfWork]
        protected virtual Task UpdateByInputAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            descriptor.ApplicationType = input.ApplicationType;
            descriptor.DisplayName = input.DisplayName;
            descriptor.ClientSecret = input.ClientSecret;
            descriptor.ConsentType = input.ConsentType;
            descriptor.ClientUri = input.ClientUri;
            descriptor.LogoUri = input.LogoUri;
            descriptor.ClientType = input.ClientType;

            foreach (var permission in input.Permissions)
            {
                descriptor.Permissions.Add(permission);
            }

            foreach (var postLogoutRedirectUri in input.PostLogoutRedirectUris)
            {
                descriptor.PostLogoutRedirectUris.Add(postLogoutRedirectUri);
            }

            foreach (var redirectUri in input.RedirectUris)
            {
                descriptor.RedirectUris.Add(redirectUri);
            }

            descriptor.Properties.Clear();
            foreach (var property in input.Properties)
            {
                descriptor.Properties.Add(property.Key, JsonSerializer.SerializeToElement(property.Value));
            }

            foreach (var requirement in input.Requirements)
            {
                descriptor.Requirements.Add(requirement);
            }

            foreach (var setting in input.Settings)
            {
                descriptor.Settings.Add(setting.Key, setting.Value);
            }

            return Task.CompletedTask;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            await Repository.DeleteAsync(entity);
        }

        [UnitOfWork]
        protected virtual Task CheckInputAsync(ApplicationCreateOrUpdateInput input)
        {
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

            return Task.CompletedTask;
        }

        [UnitOfWork]
        protected virtual async Task CheckDuplicateClientIdAsync(string clientId)
        {
            var exits = await Repository.FindByClientIdAsync(clientId);
            if (exits != null)
            {
                throw new DuplicateWarningBusinessException(nameof(OpenIddictScope.Name), clientId);
            }
        }
    }
}