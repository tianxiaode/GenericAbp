using Generic.Abp.Extensions.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Extensions;

[DependsOn(
    typeof(AbpLocalizationModule)
)]
public class GenericAbpExtensionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpExtensionsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ExtensionsResource>("en")
                .AddVirtualJson("/Generic/Abp/Extensions/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Generic.Abp.Extensions", typeof(ExtensionsResource));
            options.MapCodeNamespace("Generic.Abp.BusinessException", typeof(ExtensionsResource));
        });
    }
}