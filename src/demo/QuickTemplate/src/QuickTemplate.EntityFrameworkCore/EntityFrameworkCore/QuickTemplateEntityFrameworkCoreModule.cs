﻿using Generic.Abp.Identity.EntityFrameworkCore;
using Generic.Abp.MenuManagement;
using Generic.Abp.MenuManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickTemplate.Infrastructures.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace QuickTemplate.EntityFrameworkCore;

[DependsOn(
    typeof(QuickTemplateDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(GenericAbpIdentityEntityFrameworkCoreModule),
    typeof(QuickTemplateInfrastructuresEntityFrameworkCoreModule),
    typeof(GenericAbpMenuManagementEntityFrameworkCoreModule)
)]
public class QuickTemplateEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        QuickTemplateEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<QuickTemplateDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also QuickTemplateMigrationsDbContextFactory for EF Core tooling. */
            options.UseMySQL();
        });
    }
}