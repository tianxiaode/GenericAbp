using Generic.Abp.AuditLogging;
using Generic.Abp.Extensions;
using Generic.Abp.FileManagement;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using Localization.Resources.AbpUi;
using QuickTemplate.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(GenericAbpAuditLoggingHttpApiModule),
    typeof(GenericAbpIdentityHttpApiModule),
    typeof(GenericAbpOpenIddictHttpApiModule),
    typeof(GenericAbpExtensionsHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(GenericAbpMenuManagementHttpApiModule)
    //typeof(GenericAbpFileManagementHttpApiModule)
)]
public class QuickTemplateHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<QuickTemplateResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}