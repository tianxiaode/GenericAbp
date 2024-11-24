using Generic.Abp.Extensions;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpCachingModule),
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpExtensionsDomainModule),
        typeof(GenericAbpFileManagementDomainSharedModule),
        typeof(AbpBackgroundJobsModule),
        typeof(AbpDistributedLockingModule)
    )]
    public class GenericAbpFileManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedLockOptions>(options => { options.KeyPrefix = "FileManagement"; });
        }
    }
}