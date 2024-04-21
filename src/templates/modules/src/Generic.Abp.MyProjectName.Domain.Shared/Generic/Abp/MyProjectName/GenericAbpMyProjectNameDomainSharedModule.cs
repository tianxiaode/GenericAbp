using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.MyProjectName
{
    [DependsOn(
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpDddDomainSharedModule)
    )]
    public class GenericAbpMyProjectNameDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpMyProjectNameDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MyProjectNameResource>("en")
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddVirtualJson("/Generic/Abp/MyProjectName/Localization/MyProjectName");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.MyProjectName", typeof(MyProjectNameResource));
            });
        }
    }
}
