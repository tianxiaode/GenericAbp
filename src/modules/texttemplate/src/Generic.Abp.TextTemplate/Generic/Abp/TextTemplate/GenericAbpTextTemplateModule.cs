using Generic.Abp.TextTemplate.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.TextTemplate
{
    [DependsOn(
        typeof(AbpTextTemplatingModule),
        typeof(AbpLocalizationModule),
        typeof(AbpAutofacModule)
    )]

    public class GenericAbpTextTemplateModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpTextTemplateModule>("Generic.Abp.TextTemplate");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<GenericTextTemplateResource>("en")
                    .AddVirtualJson("/Generic/Abp/TextTemplate/Localization/GenericTextTemplate");

                //If you define this, no need to set localization resource to the templates
            });
        }

    }
}
