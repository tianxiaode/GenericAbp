﻿using Generic.Abp.AuditLogging;
using Generic.Abp.FileManagement;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace QuickTemplate;

[DependsOn(
    typeof(QuickTemplateDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(GenericAbpIdentityApplicationContractsModule),
    typeof(GenericAbpOpenIddictApplicationContractsModule),
    typeof(GenericAbpAuditLoggingApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule),
    typeof(GenericAbpMenuManagementApplicationContractsModule),
    typeof(GenericAbpFileManagementApplicationContractsModule)
)]
public class QuickTemplateApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        QuickTemplateDtoExtensions.Configure();
    }
}