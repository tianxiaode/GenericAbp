using Generic.Abp.FileManagement.EntityFrameworkCore.Files;
using Generic.Abp.FileManagement.EntityFrameworkCore.Folders;
using Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.VirtualPaths;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
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
        }
    }
}