using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.OpenIddict.Exceptions;
using Generic.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
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
        protected virtual async Task UpdateByInputAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            descriptor.ApplicationType = input.ApplicationType;
            descriptor.DisplayName = input.DisplayName;
            descriptor.ClientSecret = input.ClientSecret;
            descriptor.ConsentType = input.ConsentType;
            descriptor.ClientUri = input.ClientUri;
            descriptor.LogoUri = input.LogoUri;
            descriptor.ClientType = input.ClientType;

            //为应用添加权限
            await AssignPermissionsForFlowAsync(descriptor, input);

            //为应用添加属性
            // descriptor.Properties.Clear();
            // foreach (var property in input.Properties)
            // {
            //     descriptor.Properties.Add(property.Key, JsonSerializer.SerializeToElement(property.Value));
            // }
            //
            // //为应用添加需求
            // descriptor.Requirements.Clear();
            // descriptor.Requirements.UnionWith(input.Requirements);

            //为应用添加设置
            foreach (var setting in input.Settings)
            {
                descriptor.Settings.Add(setting.Key, setting.Value);
            }
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
            //如果ClientType不为Native，则必须指定ClientSecret
            if (string.Equals(input.ClientType, OpenIddictConstants.ClientTypes.Confidential,
                    StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrWhiteSpace(input.ClientSecret))
            {
                throw new ClientClientSecretNotSetErrorBusinessException();
            }

            if (!OpenIddictConstants.ApplicationTypes.Native.Equals(input.ApplicationType) &&
                !OpenIddictConstants.ApplicationTypes.Web.Equals(input.ApplicationType))
            {
                throw new ClientTypeErrorBusinessException(input.ApplicationType);
            }

            if (!OpenIddictConstants.ClientTypes.Confidential.Equals(input.ClientType) &&
                !OpenIddictConstants.ClientTypes.Public.Equals(input.ClientType))
            {
                throw new ClientTypeErrorBusinessException(input.ClientType);
            }

            if (!OpenIddictConstants.ConsentTypes.Implicit.Equals(input.ConsentType) &&
                !OpenIddictConstants.ConsentTypes.Systematic.Equals(input.ConsentType)
                && !OpenIddictConstants.ConsentTypes.External.Equals(input.ConsentType) &&
                !OpenIddictConstants.ConsentTypes.Explicit.Equals(input.ConsentType))
            {
                throw new ConsentTypeErrorBusinessException(input.ConsentType);
            }

            return Task.CompletedTask;
        }


        [UnitOfWork]
        // 根据input中的授权流程和ClientType，给application分配权限
        protected virtual async Task AssignPermissionsForFlowAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            descriptor.Permissions.Clear(); // 清除现有权限

            // 根据输入的 Permissions 添加相应的权限
            await AddFlowPermissionsAsync(descriptor, input);

            // 转移自定义的权限
            await AddCustomPermissionsAsync(descriptor, input);

            // 根据 ClientType 过滤不适用的 Flow
            await FilterPermissionsByClientTypeAsync(descriptor, input.ClientType);
        }

        protected virtual async Task AddFlowPermissionsAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // 通过检查 input.Permissions 来判断每个授权类型是否被允许
            var hasAuthorizationCode =
                input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
            var hasClientCredentials =
                input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
            var hasPassword = input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.Password);
            var hasRefreshToken = input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            var hasDeviceCode = input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
            var hasImplicit = input.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.Implicit);

            // Authorization Code Flow
            if (hasAuthorizationCode)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                if (input.ClientType == OpenIddictConstants.ClientTypes.Public)
                {
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                }

                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);

                await AddLogoutPermissionAndRedirectUrisAsync(descriptor, input);
            }

            // Client Credentials Flow
            if (hasClientCredentials)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
            }

            // Password Flow
            if (hasPassword)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
            }

            // Refresh Token Flow
            if (hasRefreshToken)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
            }

            // Device Flow
            if (hasDeviceCode)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            }

            // Implicit Flow
            if (hasImplicit)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
                if (input.ClientType == OpenIddictConstants.ClientTypes.Public)
                {
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                }

                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);

                await AddLogoutPermissionAndRedirectUrisAsync(descriptor, input);
            }
        }

        protected virtual Task FilterPermissionsByClientTypeAsync(AbpApplicationDescriptor descriptor,
            string clientType)
        {
            switch (clientType.ToLowerInvariant())
            {
                case OpenIddictConstants.ClientTypes.Public:
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.Password);
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);

                    // 移除与 Device Code 相关的特有权限
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Device);

                    // 公共客户端通常不使用 Introspection 或 Revocation（如果没有 Refresh Token 或 Authorization Code）
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    if (!descriptor.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.RefreshToken) &&
                        !descriptor.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode))
                    {
                        descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    }

                    break;

                case OpenIddictConstants.ClientTypes.Confidential:
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.Implicit);

                    // 移除与 Implicit Flow 相关的特有权限
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.Token);
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);

                    // 机密客户端不使用 Device Flow
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                    descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Device);
                    break;
            }

            return Task.CompletedTask;
        }

        protected virtual Task AddLogoutPermissionAndRedirectUrisAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // 检查 PostLogoutRedirectUris 是否存在并且不为空
            if (!input.PostLogoutRedirectUris.Any() ||
                !descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout))
            {
                return Task.CompletedTask;
            }

            descriptor.PostLogoutRedirectUris.Clear();
            descriptor.PostLogoutRedirectUris.UnionWith(input.PostLogoutRedirectUris);
            descriptor.RedirectUris.Clear();
            descriptor.RedirectUris.UnionWith(input.RedirectUris);
            return Task.CompletedTask;
        }

        protected virtual Task AddCustomPermissionsAsync(AbpApplicationDescriptor descriptor,
            ApplicationCreateOrUpdateInput input)
        {
            // 查找自定义权限，以 "gt:" 开头但不在标准 GrantTypes 中定义
            var defaultGrantTypes = new List<string>
            {
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.GrantTypes.Password,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                OpenIddictConstants.Permissions.GrantTypes.Implicit
            };
            var customPermissions = input.Permissions
                .Where(permission => permission.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType) &&
                                     !defaultGrantTypes.Contains(permission));

            descriptor.Permissions.UnionWith(customPermissions);

            var scopePermissions = input.Permissions
                .Where(permission => permission.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope));

            descriptor.Permissions.UnionWith(scopePermissions);
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