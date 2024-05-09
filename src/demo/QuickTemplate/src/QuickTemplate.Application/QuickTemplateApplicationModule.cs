using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using QuickTemplate.Infrastructures;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(QuickTemplateApplicationContractsModule),
    //typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(GenericAbpEnumerationApplicationModule),
    typeof(GenericAbpIdentityApplicationModule),
    typeof(GenericAbpOpenIddictApplicationModule),
    typeof(QuickTemplateInfrastructuresApplicationModule),
    typeof(GenericAbpMenuManagementApplicationModule)
)]
public class QuickTemplateApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<QuickTemplateApplicationModule>(); });
    }
}