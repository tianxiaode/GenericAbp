using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using QuickTemplate.Infrastructures;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule),
    typeof(GenericAbpEnumerationDomainSharedModule),
    typeof(GenericAbpExtResourceApplicationContractsModule),
    typeof(GenericAbpIdentityApplicationContractsModule),
    typeof(GenericAbpOpenIddictApplicationContractsModule),
    typeof(QuickTemplateInfrastructuresApplicationContractsModule),
    typeof(GenericAbpMenuManagementApplicationContractsModule)
)]
public class QuickTemplateApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        QuickTemplateDtoExtensions.Configure();
    }
}