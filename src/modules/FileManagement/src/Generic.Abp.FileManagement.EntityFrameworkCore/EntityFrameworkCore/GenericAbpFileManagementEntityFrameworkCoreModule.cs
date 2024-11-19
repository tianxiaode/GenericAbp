using Generic.Abp.FileManagement.EntityFrameworkCore.Files;
using Generic.Abp.FileManagement.EntityFrameworkCore.Folders;
using Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.VirtualPaths;
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
                options.AddRepository<File, FileRepository>();
                options.AddRepository<FilePermission, FilePermissionRepository>();
                options.AddRepository<Folder, FolderRepository>();
                options.AddRepository<FolderPermission, FolderPermissionRepository>();
                options.AddRepository<VirtualPath, VirtualPathRepository>();
                options.AddRepository<VirtualPathPermission, VirtualPathPermissionRepository>();
            });

            Configure<AbpEntityOptions>(options =>
            {
                options.Entity<Folder>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc = query => query.Include(m => m.Parent);
                });
                options.Entity<File>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc = query =>
                        query.Include(m => m.FileInfoBase).Include(m => m.Folder);
                });
            });
        }
    }
}