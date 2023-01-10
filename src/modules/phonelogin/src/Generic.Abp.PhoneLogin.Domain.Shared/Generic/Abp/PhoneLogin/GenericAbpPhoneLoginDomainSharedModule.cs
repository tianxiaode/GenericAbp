using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.PhoneLogin
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class GenericAbpPhoneLoginDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpPhoneLoginDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PhoneLoginResource>("en")
                    .AddVirtualJson("/Generic/Abp/PhoneLogin/Localization/PhoneLogin");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.PhoneLogin", typeof(PhoneLoginResource));
            });
        }
    }
}
