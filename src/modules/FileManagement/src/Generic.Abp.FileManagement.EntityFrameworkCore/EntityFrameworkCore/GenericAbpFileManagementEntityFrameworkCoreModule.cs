﻿using Generic.Abp.FileManagement.EntityFrameworkCore.FileInfoBases;
using Generic.Abp.FileManagement.EntityFrameworkCore.Resources;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpFileManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpFileManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FileManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<FileInfoBase, FileInfoBaseRepository>();
                options.AddRepository<Resource, ResourceRepository>();
                options.AddRepository<ResourcePermission, ResourcePermissionRepository>();
            });

            Configure<AbpEntityOptions>(options =>
            {
                options.Entity<Resource>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc = query =>
                        query.Include<Resource, Resource?>(m => m.Parent)
                            .Include<Resource, FileInfoBase?>(m => m.FileInfoBase);
                });
            });
        }
    }
}