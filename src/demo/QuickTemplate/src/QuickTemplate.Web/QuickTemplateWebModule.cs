using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using QuickTemplate.EntityFrameworkCore;
using QuickTemplate.Localization;
using QuickTemplate.MultiTenancy;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.ExternalAuthentication;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace QuickTemplate.Web;

[DependsOn(
    typeof(QuickTemplateHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(QuickTemplateApplicationModule),
    typeof(QuickTemplateEntityFrameworkCoreModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    //typeof(GenericAbpTailwindModule),
    typeof(GenericAbpExternalAuthenticationAspNetCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class QuickTemplateWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
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

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            //builder.AddServer(m =>
            //{
            //    m.SetAccessTokenLifetime(TimeSpan.FromDays(3));
            //    m.SetRefreshTokenLifetime(TimeSpan.FromDays(4));
            //});
            builder.AddValidation(options =>
            {
                options.AddAudiences("QuickTemplate"); // Replace with your application Name
                options.UseLocalServer();
                //options.EnableAuthorizationEntryValidation();
                //options.EnableTokenEntryValidation();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureExternalProviders(context, configuration);
        //ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        //ConfigureLocalizationServices();
        //ConfigureNavigationServices();
        //ConfigureAutoApiControllers();
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context.Services, configuration);

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
                options.ClientId = configuration["Authentication:GitHub:ClientId"] ?? "";
                options.ClientSecret = configuration["Authentication:GitHub:ClientSecret"] ?? "";
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = configuration["Authentication:Microsoft:ClientId"] ?? "";
                options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"] ?? "";
            });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ??
                                                 new string[] { });

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
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
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }


    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();

        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }


        //app.UseForwardedHeaders(new ForwardedHeadersOptions
        //{
        //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        //});


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
    }
}