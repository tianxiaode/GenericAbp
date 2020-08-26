using Generic.Abp.TextTemplate;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Account
{
    [DependsOn(
        typeof(GenericAccountApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(GenericAbpTextTemplateModule),
        typeof(AbpMailKitModule),
        typeof(AbpCachingModule)
    )]
    public class GenericAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAccountApplicationModule>();
            });
        }
    }
}
