using Generic.Abp.FileManagement.EntityFrameworkCore.FileInfoBases;
using Generic.Abp.FileManagement.EntityFrameworkCore.Resources;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Generic.Abp.FileManagement.EntityFrameworkCore.ExternalShares;
using Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;
using Generic.Abp.FileManagement.ExternalShares;
using Generic.Abp.FileManagement.VirtualPaths;
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
                options.AddRepository<VirtualPath, VirtualPathRepository>();
                options.AddRepository<ExternalShare, ExternalShareRepository>();
            });

            Configure<AbpEntityOptions>(options =>
            {
                options.Entity<Resource>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc =
                        new Func<IQueryable<Resource>, IQueryable<Resource>>(query =>
                            query.Include(m => m.Parent)
                        );
                });

                options.Entity<VirtualPath>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc =
                        new Func<IQueryable<VirtualPath>, IQueryable<VirtualPath>>(query =>
                            query.Include(m => m.Resource)
                        );
                });

                options.Entity<ExternalShare>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc =
                        new Func<IQueryable<ExternalShare>, IQueryable<ExternalShare>>(query =>
                            query.Include(m => m.Resource)
                        );
                });
            });
        }
    }
}