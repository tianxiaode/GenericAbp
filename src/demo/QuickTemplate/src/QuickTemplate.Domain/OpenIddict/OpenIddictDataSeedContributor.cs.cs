using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace QuickTemplate.OpenIddict
{
    /* Creates initial data that is needed to property run the application
     * and make client-to-server communication possible.
     */
    public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IAbpApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IStringLocalizer<OpenIddictResponse> L;

        public OpenIddictDataSeedContributor(
            IConfiguration configuration,
            IAbpApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IPermissionDataSeeder permissionDataSeeder,
            IStringLocalizer<OpenIddictResponse> l)
        {
            _configuration = configuration;
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _permissionDataSeeder = permissionDataSeeder;
            L = l;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateScopesAsync();
            await CreateApplicationsAsync();
        }

        private async Task CreateScopesAsync()
        {
            if (await _scopeManager.FindByNameAsync("QuickTemplate") == null)
            {
                await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "QuickTemplate",
                    DisplayName = "QuickTemplate API",
                    Resources =
                    {
                        "QuickTemplate"
                    }
                });
            }
        }

        private async Task CreateApplicationsAsync()
        {
            var commonScopes = new List<string>
            {
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Phone,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles,
                "QuickTemplate"
            };

            var configurationSection = _configuration.GetSection("OpenIddict:Applications");

            //Web Client
            var webClientId = configurationSection["QuickTemplate_Web:ClientId"];
            if (!webClientId.IsNullOrWhiteSpace())
            {
                var webClientRootUrl = configurationSection["QuickTemplate_Web:RootUrl"].EnsureEndsWith('/');

                /* QuickTemplate_Web client is only needed if you created a tiered
                 * solution. Otherwise, you can delete this client. */
                await CreateApplicationAsync(
                    name: webClientId,
                    applicationType: OpenIddictConstants.ApplicationTypes.Web,
                    clientType: OpenIddictConstants.ClientTypes.Confidential,
                    consentType: OpenIddictConstants.ConsentTypes.Implicit,
                    displayName: "Web Application",
                    secret: configurationSection["QuickTemplate_Web:ClientSecret"] ?? "1q2w3e*",
                    grantTypes: new List<string> //Hybrid flow
                    {
                        OpenIddictConstants.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.GrantTypes.Implicit
                    },
                    scopes: commonScopes,
                    redirectUri: new List<string>() { $"{webClientRootUrl}signin-oidc" },
                    postLogoutRedirectUri: new List<string>() { $"{webClientRootUrl}signout-callback-oidc" },
                    clientUri: webClientRootUrl);
            }

            //Console Test / Angular Client
            var consoleAndAngularClientId = configurationSection["QuickTemplate_App:ClientId"];
            if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
            {
                var consoleAndAngularClientRootUrl = configurationSection["QuickTemplate_App:RootUrl"]?.TrimEnd('/');
                var urls = new List<string>()
                {
                    consoleAndAngularClientRootUrl,
                    consoleAndAngularClientRootUrl + "/desktop",
                    consoleAndAngularClientRootUrl + "/phone"
                };
                await CreateApplicationAsync(
                    name: consoleAndAngularClientId,
                    applicationType: OpenIddictConstants.ApplicationTypes.Web,
                    clientType: OpenIddictConstants.ClientTypes.Public,
                    consentType: OpenIddictConstants.ConsentTypes.Implicit,
                    displayName: "Console Test / Angular Application",
                    secret: null,
                    grantTypes: new List<string>
                    {
                        OpenIddictConstants.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.GrantTypes.Password,
                        OpenIddictConstants.GrantTypes.ClientCredentials,
                        OpenIddictConstants.GrantTypes.RefreshToken
                    },
                    scopes: commonScopes,
                    redirectUri: urls,
                    postLogoutRedirectUri: urls, clientUri: consoleAndAngularClientRootUrl);
            }


            // Swagger Client
            var swaggerClientId = configurationSection["QuickTemplate_Swagger:ClientId"];
            if (!swaggerClientId.IsNullOrWhiteSpace())
            {
                var swaggerRootUrl = configurationSection["QuickTemplate_Swagger:RootUrl"].TrimEnd('/');

                await CreateApplicationAsync(
                    name: swaggerClientId,
                    applicationType: OpenIddictConstants.ApplicationTypes.Web,
                    clientType: OpenIddictConstants.ClientTypes.Public,
                    consentType: OpenIddictConstants.ConsentTypes.Implicit,
                    displayName: "Swagger Application",
                    secret: null,
                    grantTypes: new List<string>
                    {
                        OpenIddictConstants.GrantTypes.AuthorizationCode,
                    },
                    scopes: commonScopes,
                    redirectUri: new List<string>() { $"{swaggerRootUrl}/swagger/oauth2-redirect.html" },
                    clientUri: swaggerRootUrl,
                    postLogoutRedirectUri: new List<string>()
                );
            }
        }

        private async Task CreateApplicationAsync([NotNull] string name,
            [NotNull] string applicationType,
            [NotNull] string clientType,
            [NotNull] string consentType,
            string displayName,
            string secret,
            List<string> grantTypes,
            List<string> scopes,
            List<string> redirectUri,
            List<string> postLogoutRedirectUri,
            List<string> permissions = null,
            string clientUri = null)
        {
            if (!string.IsNullOrEmpty(secret) && string.Equals(clientType, OpenIddictConstants.ClientTypes.Public,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new Volo.Abp.BusinessException(L["NoClientSecretCanBeSetForPublicApplications"]);
            }

            if (string.IsNullOrEmpty(secret) && string.Equals(clientType, OpenIddictConstants.ClientTypes.Confidential,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new Volo.Abp.BusinessException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
            }

            if (!string.IsNullOrEmpty(name) && await _applicationManager.FindByClientIdAsync(name) != null)
            {
                return;
                //throw new BusinessException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
            }

            var client = await _applicationManager.FindByClientIdAsync(name);
            if (client == null)
            {
                var application = new AbpApplicationDescriptor
                {
                    ApplicationType = applicationType,
                    ClientId = name,
                    ClientType = clientType,
                    ClientSecret = secret,
                    ConsentType = consentType,
                    DisplayName = displayName,
                    ClientUri = clientUri,
                };

                Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
                Check.NotNullOrEmpty(scopes, nameof(scopes));

                if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }
                    .All(grantTypes.Contains))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

                    if (string.Equals(clientType, OpenIddictConstants.ClientTypes.Public,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                    }
                }

                if (redirectUri.Count > 0 || postLogoutRedirectUri.Count > 0)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
                }

                foreach (var grantType in grantTypes)
                {
                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                        grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                        grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                        grantType == OpenIddictConstants.GrantTypes.Password ||
                        grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                        grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Password)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                        if (string.Equals(clientType, OpenIddictConstants.ClientTypes.Public,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                        }
                    }
                }

                var buildInScopes = new[]
                {
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles
                };

                foreach (var scope in scopes)
                {
                    if (buildInScopes.Contains(scope))
                    {
                        application.Permissions.Add(scope);
                    }
                    else
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                    }
                }

                if (redirectUri.Count > 0)
                {
                    foreach (var url in redirectUri)
                    {
                        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                            !uri.IsWellFormedOriginalString())
                        {
                            throw new Volo.Abp.BusinessException(L["InvalidPostLogoutRedirectUri",
                                postLogoutRedirectUri]);
                        }

                        if (application.RedirectUris.All(x => x != uri))
                        {
                            application.RedirectUris.Add(uri);
                        }
                    }
                }

                if (postLogoutRedirectUri.Count > 0)
                {
                    foreach (var url in postLogoutRedirectUri)
                    {
                        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                            !uri.IsWellFormedOriginalString())
                        {
                            throw new Volo.Abp.BusinessException(L["InvalidPostLogoutRedirectUri",
                                postLogoutRedirectUri]);
                        }

                        if (application.PostLogoutRedirectUris.All(x => x != uri))
                        {
                            application.PostLogoutRedirectUris.Add(uri);
                        }
                    }
                }

                if (permissions != null)
                {
                    await _permissionDataSeeder.SeedAsync(
                        ClientPermissionValueProvider.ProviderName,
                        name,
                        permissions,
                        null
                    );
                }

                await _applicationManager.CreateAsync(application);
            }
        }
    }
}