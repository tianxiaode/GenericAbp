using Generic.Abp.Tailwind.Bundling;
using Generic.Abp.Tailwind.Localization;
using Generic.Abp.Tailwind.OpenIddict;
using Generic.Abp.Tailwind.OpenIddict.Claims;
using Generic.Abp.Tailwind.OpenIddict.Scopes;
using Generic.Abp.Tailwind.OpenIddict.WildcardDomains;
using Microsoft.AspNetCore.Mvc.Razor;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Tailwind;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpExceptionHandlingModule)
)]
public class GenericAbpTailwindModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(TailwindResource),
                typeof(GenericAbpTailwindModule).Assembly);
        });


        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpTailwindModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<TailwindTheme>();

            options.DefaultThemeName ??= TailwindTheme.Name;
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpTailwindModule>(
                "Generic.Abp.Tailwind");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TailwindResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddBaseTypes(typeof(AccountResource))
                .AddVirtualJson("/Localization/Resources");
        });


        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Generic.Abp.Tailwind", typeof(TailwindResource));
        });


        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(TailwindThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailwindGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(TailwindThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailwindGlobalScriptContributor));
                });
        });


        context.Services.AddAutoMapperObjectMapper<GenericAbpTailwindModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddProfile<TailwindAutoMapperProfile>(validate: true); });

        AddOpenIddictServer(context.Services);

        Configure<AbpOpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<AbpDynamicClaimsOpenIddictClaimsPrincipalHandler>();
            options.ClaimsPrincipalHandlers.Add<AbpDefaultOpenIddictClaimsPrincipalHandler>();
        });

        Configure<RazorViewEngineOptions>(options => { options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml"); });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() => { });
    }


    private void AddOpenIddictServer(IServiceCollection services)
    {
        var builderOptions = services.ExecutePreConfiguredActions<AbpOpenIddictAspNetCoreOptions>();

        if (builderOptions.UpdateAbpClaimTypes)
        {
            AbpClaimTypes.UserId = OpenIddictConstants.Claims.Subject;
            AbpClaimTypes.Role = OpenIddictConstants.Claims.Role;
            AbpClaimTypes.UserName = OpenIddictConstants.Claims.PreferredUsername;
            AbpClaimTypes.Name = OpenIddictConstants.Claims.GivenName;
            AbpClaimTypes.SurName = OpenIddictConstants.Claims.FamilyName;
            AbpClaimTypes.PhoneNumber = OpenIddictConstants.Claims.PhoneNumber;
            AbpClaimTypes.PhoneNumberVerified = OpenIddictConstants.Claims.PhoneNumberVerified;
            AbpClaimTypes.Email = OpenIddictConstants.Claims.Email;
            AbpClaimTypes.EmailVerified = OpenIddictConstants.Claims.EmailVerified;
            AbpClaimTypes.ClientId = OpenIddictConstants.Claims.ClientId;
        }

        var openIddictBuilder = services.AddOpenIddict()
            .AddServer(builder =>
            {
                builder
                    .SetAuthorizationEndpointUris("connect/authorize", "connect/authorize/callback")
                    // .well-known/oauth-authorization-server
                    // .well-known/openid-configuration
                    //.SetConfigurationEndpointUris()
                    // .well-known/jwks
                    //.SetCryptographyEndpointUris()
                    .SetDeviceEndpointUris("device")
                    .SetIntrospectionEndpointUris("connect/introspect")
                    .SetLogoutEndpointUris("connect/logout")
                    .SetRevocationEndpointUris("connect/revocat")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserinfoEndpointUris("connect/userinfo")
                    .SetVerificationEndpointUris("connect/verify");

                builder
                    .AllowAuthorizationCodeFlow()
                    .AllowHybridFlow()
                    .AllowImplicitFlow()
                    .AllowPasswordFlow()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow()
                    .AllowDeviceCodeFlow()
                    .AllowNoneFlow();

                builder.RegisterScopes(new[]
                {
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess
                });

                builder.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();

                if (builderOptions.AddDevelopmentEncryptionAndSigningCertificate)
                {
                    builder
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }

                builder.DisableAccessTokenEncryption();

                var wildcardDomainsOptions = services.ExecutePreConfiguredActions<AbpOpenIddictWildcardDomainOptions>();
                if (wildcardDomainsOptions.EnableWildcardDomainSupport)
                {
                    var preActions = services.GetPreConfigureActions<AbpOpenIddictWildcardDomainOptions>();

                    Configure<AbpOpenIddictWildcardDomainOptions>(options => { preActions.Configure(options); });

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri
                        .Descriptor);
                    builder.AddEventHandler(AbpValidateClientRedirectUri.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Authentication.ValidateRedirectUriParameter
                        .Descriptor);
                    builder.AddEventHandler(AbpValidateRedirectUriParameter.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri
                        .Descriptor);
                    builder.AddEventHandler(AbpValidateClientPostLogoutRedirectUri.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter
                        .Descriptor);
                    builder.AddEventHandler(AbpValidatePostLogoutRedirectUriParameter.Descriptor);

                    builder.RemoveEventHandler(OpenIddictServerHandlers.Session.ValidateAuthorizedParty.Descriptor);
                    builder.AddEventHandler(AbpValidateAuthorizedParty.Descriptor);
                }

                builder.AddEventHandler(RemoveClaimsFromClientCredentialsGrantType.Descriptor);
                builder.AddEventHandler(AttachScopes.Descriptor);

                services.ExecutePreConfiguredActions(builder);
            });

        services.ExecutePreConfiguredActions(openIddictBuilder);
    }
}