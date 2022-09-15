using AutoMapper;
using Generic.Abp.IdentityServer.ApiResources;
using Generic.Abp.IdentityServer.ApiScopes;
using Generic.Abp.IdentityServer.Clients;
using Generic.Abp.IdentityServer.IdentityResources;
using Generic.Abp.IdentityServer.Secrets;
using Generic.Abp.IdentityServer.UserClaims;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Generic.Abp.IdentityServer
{
    public class IdentityServerApplicationAutoMapperProfile : Profile
    {
        public IdentityServerApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<IdentityResource, IdentityResourceDto>();
            CreateMap<IdentityResourceClaim, IdentityResourceClaimDto>();
            CreateMap<ApiResource, ApiResourceDto>();
            CreateMap<ApiResourceClaim, ApiResourceClaimDto>();
            CreateMap<ApiResourceSecret, ApiResourceSecretDto>();
            CreateMap<ApiScope, ApiScopeDto>();
            CreateMap<ApiScopeClaim, ApiScopeClaimDto>();
            CreateMap<Client, ClientDto>();
            CreateMap<ClientCreateInput, Client>()
                .Ignore(m => m.Id)
                .Ignore(m => m.RequireRequestObject)
                .Ignore(m => m.AllowedIdentityTokenSigningAlgorithms)
                .Ignore(m => m.AllowedScopes)
                .Ignore(m => m.ClientSecrets)
                .Ignore(m => m.AllowedGrantTypes)
                .Ignore(m => m.AllowedCorsOrigins)
                .Ignore(m => m.RedirectUris)
                .Ignore(m => m.PostLogoutRedirectUris)
                .Ignore(m => m.IdentityProviderRestrictions)
                .Ignore(m => m.Claims)
                .Ignore(m => m.Properties)
                .Ignore(m => m.IsDeleted)
                .Ignore(m => m.DeleterId)
                .Ignore(m => m.DeletionTime)
                .Ignore(m => m.LastModificationTime)
                .Ignore(m => m.LastModifierId)
                .Ignore(m => m.CreationTime)
                .Ignore(m => m.CreatorId)
                .Ignore(m => m.ExtraProperties)
                .Ignore(m => m.ConcurrencyStamp);
            CreateMap<ClientCorsOrigin, ClientCorsOriginDto>();
            CreateMap<ClientGrantType, ClientGrantTypeDto>();
            CreateMap<ClientScope, ClientScopeDto>();
            CreateMap<ClientSecret, ClientSecretDto>();
            CreateMap<ClientClaim, ClientClaimDto>();
            CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriDto>();
            CreateMap<ClientRedirectUri, ClientRedirectUriDto>();
        }
    }
}