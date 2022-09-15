using Volo.Abp.FeatureManagement;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Application;

[DependsOn(
    typeof(AbpMailKitModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
)]
public class GenericAbpApplicationModule : AbpModule
{
}