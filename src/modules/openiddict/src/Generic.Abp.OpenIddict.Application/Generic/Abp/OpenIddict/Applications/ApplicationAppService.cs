using Generic.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

namespace Generic.Abp.OpenIddict.Applications
{
    [RemoteService(false)]
    public class ApplicationAppService : OpenIddictAppService, IApplicationAppService
    {
        public ApplicationAppService(IOpenIddictApplicationRepository openIddictApplicationRepository,
            IOpenIddictScopeRepository scopeRepository)
        {
            Repository = openIddictApplicationRepository;
            ScopeRepository = scopeRepository;
        }

        protected IOpenIddictApplicationRepository Repository { get; }
        protected IOpenIddictScopeRepository ScopeRepository { get; }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<ApplicationDto> GetAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity);
        }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input)
        {
            var sorting = input.Sorting;
            if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(OpenIddictApplication.ClientId)}";
            var list = await Repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
            var count = await Repository.GetCountAsync();
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
        public virtual async Task<ListResultDto<ApplicationDto>> DeleteAsync(List<Guid> ids)
        {
            var result = new List<ApplicationDto>();
            foreach (var guid in ids)
            {
                var entity = await Repository.FindAsync(guid);
                if (entity == null) continue;
                await Repository.DeleteAsync(entity);
                result.Add(ObjectMapper.Map<OpenIddictApplication, ApplicationDto>(entity));
            }


            return new ListResultDto<ApplicationDto>(result);
        }

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<Dictionary<string, Dictionary<string, string>>> GetAllPermisions()
        {
            var result = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    nameof(OpenIddictConstants.Permissions.Endpoints),
                    new Dictionary<string, string>
                    {
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Authorization),
                            OpenIddictConstants.Permissions.Endpoints.Authorization
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Device),
                            OpenIddictConstants.Permissions.Endpoints.Device
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Introspection),
                            OpenIddictConstants.Permissions.Endpoints.Introspection
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Logout),
                            OpenIddictConstants.Permissions.Endpoints.Logout
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Revocation),
                            OpenIddictConstants.Permissions.Endpoints.Revocation
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Endpoints.Token),
                            OpenIddictConstants.Permissions.Endpoints.Token
                        }
                    }
                },
                {
                    nameof(OpenIddictConstants.Permissions.GrantTypes),
                    new Dictionary<string, string>
                    {
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode),
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials),
                            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.DeviceCode),
                            OpenIddictConstants.Permissions.GrantTypes.DeviceCode
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.Implicit),
                            OpenIddictConstants.Permissions.GrantTypes.Implicit
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.Password),
                            OpenIddictConstants.Permissions.GrantTypes.Password
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.GrantTypes.RefreshToken),
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken
                        }
                    }
                },
                {
                    nameof(OpenIddictConstants.Permissions.ResponseTypes),
                    new Dictionary<string, string>
                    {
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.Code),
                            OpenIddictConstants.Permissions.ResponseTypes.Code
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken),
                            OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken),
                            OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.CodeToken),
                            OpenIddictConstants.Permissions.ResponseTypes.CodeToken
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.IdToken),
                            OpenIddictConstants.Permissions.ResponseTypes.IdToken
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken),
                            OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.None),
                            OpenIddictConstants.Permissions.ResponseTypes.None
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.ResponseTypes.Token),
                            OpenIddictConstants.Permissions.ResponseTypes.Token
                        }
                    }
                },
                {
                    nameof(OpenIddictConstants.Permissions.Scopes),
                    new Dictionary<string, string>
                    {
                        {
                            nameof(OpenIddictConstants.Permissions.Scopes.Address),
                            OpenIddictConstants.Permissions.Scopes.Address
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Scopes.Email),
                            OpenIddictConstants.Permissions.Scopes.Email
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Scopes.Phone),
                            OpenIddictConstants.Permissions.Scopes.Phone
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Scopes.Profile),
                            OpenIddictConstants.Permissions.Scopes.Profile
                        },
                        {
                            nameof(OpenIddictConstants.Permissions.Scopes.Roles),
                            OpenIddictConstants.Permissions.Scopes.Roles
                        },
                    }
                }
            };
            var scopePermissions = result[nameof(OpenIddictConstants.Permissions.Scopes)];
            var scopes = await ScopeRepository.GetListAsync();
            foreach (var scope in scopes)
            {
                scopePermissions.Add(scope.Name, $"scp:{scope.Name}");
            }

            return result;
        }

        #region permissions

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<List<string>> GetPermissionsAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var permissions = entity.Permissions.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Permissions,
                    new System.Text.Json.JsonSerializerOptions());
            return permissions;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task AddPermissionAsync(Guid id, PermissionCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var permissions = entity.Permissions.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Permissions,
                    new System.Text.Json.JsonSerializerOptions());
            if (permissions.Any(m => m.Equals(input.Value))) return;
            permissions.Add(input.Value);
            entity.Permissions = System.Text.Json.JsonSerializer.Serialize(permissions);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task RemovePermissionAsync(Guid id, PermissionDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var permissions = entity.Permissions.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Permissions,
                    new System.Text.Json.JsonSerializerOptions());
            permissions.Remove(input.Value);
            entity.Permissions = System.Text.Json.JsonSerializer.Serialize(permissions);
            await Repository.UpdateAsync(entity);
        }

        #endregion

        #region PostLogoutRedirectUris

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<List<string>> GetPostLogoutRedirectUrisAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.PostLogoutRedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.PostLogoutRedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            return list;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task AddPostLogoutRedirectUriAsync(Guid id,
            PostLogoutRedirectUriCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.PostLogoutRedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.PostLogoutRedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            if (list.Any(m => m.Equals(input.Value))) return;
            list.Add(input.Value);
            entity.PostLogoutRedirectUris = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task RemovePostLogoutRedirectUriAsync(Guid id,
            PostLogoutRedirectUriDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.PostLogoutRedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.PostLogoutRedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            foreach (var item in input.Items)
            {
                list.Remove(item);
            }

            entity.PostLogoutRedirectUris = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        #endregion

        #region Properties

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
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
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task AddPropertyAsync(Guid id, PropertyCreateInput input)
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
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task RemovePropertyAsync(Guid id, PropertyDeleteInput input)
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

        #region RedirectUris

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<List<string>> GetRedirectUrisAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.RedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.RedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            return list;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task AddRedirectUriAsync(Guid id, RedirectUriCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.RedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.RedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            if (list.Any(m => m.Equals(input.Value))) return;
            list.Add(input.Value);
            entity.RedirectUris = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task RemoveRedirectUriAsync(Guid id, RedirectUriDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.RedirectUris.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.RedirectUris,
                    new System.Text.Json.JsonSerializerOptions());
            foreach (var item in input.Items)
            {
                list.Remove(item);
            }

            entity.RedirectUris = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        #endregion

        #region Requirements

        [UnitOfWork]
        [DisableEntityChangeTracking]
        [Authorize(OpenIddictPermissions.Applications.Default)]
        public virtual async Task<List<string>> GetRequirementsAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Requirements.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Requirements,
                    new System.Text.Json.JsonSerializerOptions());
            return list;
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task AddRequirementAsync(Guid id, RequirementCreateInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Requirements.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Requirements,
                    new System.Text.Json.JsonSerializerOptions());
            if (list.Any(m => m.Equals(input.Value))) return;
            list.Add(input.Value);
            entity.Requirements = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        [UnitOfWork]
        [Authorize(OpenIddictPermissions.Applications.Update)]
        public virtual async Task RemoveRequirementAsync(Guid id, RequirementDeleteInput input)
        {
            var entity = await Repository.GetAsync(id);
            var list = entity.Requirements.IsNullOrEmpty()
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(entity.Requirements,
                    new System.Text.Json.JsonSerializerOptions());
            foreach (var item in input.Items)
            {
                list.Remove(item);
            }

            entity.Requirements = System.Text.Json.JsonSerializer.Serialize(list);
            await Repository.UpdateAsync(entity);
        }

        #endregion
    }
}