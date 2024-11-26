using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions;
using Generic.Abp.FileManagement.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpCachingModule),
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpExtensionsDomainModule),
        typeof(GenericAbpFileManagementDomainSharedModule),
        typeof(AbpBackgroundJobsModule)
        //typeof(AbpDistributedLockingModule)
    )]
    public class GenericAbpFileManagementDomainModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<AbpDistributedLockOptions>(options => { options.KeyPrefix = "FileManagement"; });

            //context.Services.AddHostedService<ScheduledTaskHostService>();
        }
    }
}