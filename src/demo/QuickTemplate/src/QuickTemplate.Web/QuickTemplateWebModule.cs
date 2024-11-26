using Generic.Abp.ExternalAuthentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using QuickTemplate.EntityFrameworkCore;
using QuickTemplate.Localization;
using QuickTemplate.MultiTenancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog.Core;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Security;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.PermissionManagement;
using Microsoft.AspNetCore.Authentication.OAuth;
using AspNet.Security.OAuth.GitHub;

namespace QuickTemplate.Web;

[DependsOn(
    typeof(QuickTemplateHttpApiModule),
    typeof(GenericAbpExternalAuthenticationAspNetCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(QuickTemplateApplicationModule),
    typeof(QuickTemplateEntityFrameworkCoreModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class QuickTemplateWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            // builder.AddServer(m =>
            // {
            //     m.SetAccessTokenLifetime(TimeSpan.FromDays(3));
            //     m.SetRefreshTokenLifetime(TimeSpan.FromDays(4));
            // });
            builder.AddValidation(options =>
            {
                options.AddAudiences("QuickTemplate"); // Replace with your application Name
                options.UseLocalServer();
                options.EnableAuthorizationEntryValidation();
                options.EnableTokenEntryValidation();
                options.UseAspNetCore();
            });
        });

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(QuickTemplateResource),
                typeof(QuickTemplateDomainModule).Assembly,
                typeof(QuickTemplateDomainSharedModule).Assembly,
                typeof(QuickTemplateApplicationModule).Assembly,
                typeof(QuickTemplateApplicationContractsModule).Assembly,
                typeof(QuickTemplateWebModule).Assembly
            );
        });

        if (hostingEnvironment.IsDevelopment())
        {
            return;
        }

        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
        {
            serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx",
                "dd0c34ad-293e-4c9c-865e-3334a4b16b0c");
            serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]!));
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        }

        if (!configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"))
        {
            Configure<OpenIddictServerAspNetCoreOptions>(options =>
            {
                options.DisableTransportSecurityRequirement = true;
            });

            Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });
        }


        ConfigureExternalProviders(context, configuration);
        //ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureAuthentication(context);
        ConfigureAutoMapper();
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(hostingEnvironment);
        //ConfigureLocalizationServices();
        //ConfigureNavigationServices();
        //ConfigureAutoApiControllers();
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context.Services, configuration);

        Configure<PermissionManagementOptions>(options => { options.IsDynamicPermissionStoreEnabled = true; });
        Configure<AbpClockOptions>(options => { options.Kind = DateTimeKind.Utc; });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults
            .AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureExternalProviders(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()
            .AddGitHub(options =>
            {
                options.ClientId = "ddd";
                options.ClientSecret = "ddd";
            })
            .AddGitee(options =>
            {
                options.ClientId = configuration["Authentication:Gitee:ClientId"] ?? "";
                options.ClientSecret = configuration["Authentication:Gitee:ClientSecret"] ?? "";
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId =
                    configuration["Authentication:Microsoft:ClientId"] ?? "";
                options.ClientSecret =
                    configuration["Authentication:Microsoft:ClientSecret"] ?? "";
            });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? []);

            options.Applications["Angular"].RootUrl = configuration["App:CorsOrigins"];
            //options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "reset-password";
            options.Applications["QuickTemplate"].RootUrl = configuration["App:CorsOrigins"];
            options.Applications["QuickTemplate"].Urls.Add(AccountUrlNames.PasswordReset, "reset-password");
        });
    }

    private void ConfigureBundles()
    {
        //Configure<AbpBundlingOptions>(options =>
        //{
        //    options.StyleBundles.Configure(
        //        BasicThemeBundles.Styles.Global,
        //        bundle => { bundle.AddFiles("/global-styles.css"); }
        //    );
        //});
    }


    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<QuickTemplateWebModule>(); });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<QuickTemplateDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}QuickTemplate.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<QuickTemplateDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}QuickTemplate.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<QuickTemplateApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}QuickTemplate.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<QuickTemplateApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}QuickTemplate.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<QuickTemplateWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }


    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(QuickTemplateApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                { "QuickTemplate", "QuickTemplate API" }
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "QuickTemplate API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var corsOrigins = configuration["App:CorsOrigins"]
            ?.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(o => o.TrimEnd('/'))
            .ToArray() ?? [];

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(corsOrigins)
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });


        // ≈‰÷√ AbpSecurityHeadersOptions
        // Configure<AbpSecurityHeadersOptions>(options =>
        // {
        //     if (corsOrigins.Length > 0)
        //     {
        //         options.Headers["X-Frame-Options"] = "frame-ancestors 'self' " + string.Join(" ", corsOrigins);
        //     }
        // });
    }


    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.GetConfiguration();

        app.UseForwardedHeaders();
        // app.UseForwardedHeaders(new ForwardedHeadersOptions
        // {
        //     ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        // });
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        //app.UseAbpSecurityHeaders();
        app.UseCors();
        app.UseAbpSecurityHeaders();
        app.UseAuthentication();

        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }


        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickTemplate API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthScopes("QuickTemplate");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        // OneTimeRunner.Run(() =>
        // {
        //     var hostedService = context.ServiceProvider.GetService<ExternalProviderUpdaterService>();
        //     hostedService?.StartAsync(default).GetAwaiter().GetResult();
        // });
    }
}