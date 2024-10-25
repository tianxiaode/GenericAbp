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
        // 根据input中的授权流程和ClientType，给application分配权限
        protected virtual async Task AssignPermissionsForFlowAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // 如果 AllowHybridFlow 为 true，则将 AuthorizationCodeFlow 和 ImplicitFlow 都启用
            if (input.EnableHybridFlow)
            {
                input.EnableAuthorizationCodeFlow = true;
                input.EnableImplicitFlow = true;
            }

            switch (input.ClientType.ToLowerInvariant())
            {
                case OpenIddictConstants.ClientTypes.Public:
                    await ConfigurePublicClientAsync(descriptor, input);
                    break;
                case OpenIddictConstants.ClientTypes.Confidential:
                    await ConfigureConfidentialClientAsync(descriptor, input);
                    break;
            }

            // 密码模式（Password Flow）
            if (input.EnablePasswordFlow)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }

            // 刷新令牌模式（Refresh Token Flow）
            if (input.EnableRefreshTokenFlow)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }

            // 设备代码模式（Device Code Flow）
            if (input.EnableDeviceFlow)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device); // 添加 Device 端点权限
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }
        }

        protected virtual async Task ConfigurePublicClientAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // Public clients typically support only Authorization Code Flow (with PKCE) and Implicit Flow
            if (input.EnableAuthorizationCodeFlow)
            {
                await AddAuthorizationCodePermissionsAsync(descriptor);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints
                    .Authorization); // 添加 Authorization 端点权限
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }

            if (input.EnableImplicitFlow)
            {
                await AddImplicitPermissionsAsync(descriptor);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints
                    .Authorization); // 添加 Authorization 端点权限
            }

            // Public clients typically cannot use Client Credentials or Password flows
            input.EnableClientCredentialsFlow = false;
            input.EnablePasswordFlow = false;
        }

        protected virtual async Task ConfigureConfidentialClientAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // Confidential clients support Authorization Code Flow and Client Credentials Flow
            if (input.EnableAuthorizationCodeFlow)
            {
                await AddAuthorizationCodePermissionsAsync(descriptor);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints
                    .Authorization); // 添加 Authorization 端点权限
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }

            if (input.EnableClientCredentialsFlow)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token); // 添加 Token 端点权限
            }

            // Confidential clients usually do not use Implicit Flow
            input.EnableImplicitFlow = false;
        }

        protected virtual async Task AddAuthorizationCodePermissionsAsync(AbpApplicationDescriptor descriptor)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);

            await Task.CompletedTask; // 确保方法返回 Task
        }

        protected virtual async Task AddImplicitPermissionsAsync(AbpApplicationDescriptor descriptor)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);

            await Task.CompletedTask; // 确保方法返回 Task
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