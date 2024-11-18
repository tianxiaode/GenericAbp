using Generic.Abp.AuditLogging;
using Generic.Abp.FileManagement;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(QuickTemplateApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(GenericAbpIdentityApplicationModule),
    typeof(GenericAbpOpenIddictApplicationModule),
    typeof(GenericAbpAuditLoggingApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(GenericAbpMenuManagementApplicationModule),
    typeof(GenericAbpFileManagementApplicationModule)
)]
public class QuickTemplateApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<QuickTemplateApplicationModule>(); });
    }
}