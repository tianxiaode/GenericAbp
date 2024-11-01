using System;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.MultiLingual;
using Generic.Abp.Extensions.Entities.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement;

public class MenuDataSeed : ITransientDependency, IMenuDataSeed
{
    public MenuDataSeed(IGuidGenerator guidGenerator, IUnitOfWork currentUnitOfWork, ILogger<IMenuDataSeed> logger,
        MenuManager menuManager)
    {
        GuidGenerator = guidGenerator;
        CurrentUnitOfWork = currentUnitOfWork;
        Logger = logger;
        MenuManager = menuManager;
    }

    protected IGuidGenerator GuidGenerator { get; }
    protected MenuManager MenuManager { get; }
    protected IUnitOfWork CurrentUnitOfWork { get; }
    protected ILogger<IMenuDataSeed> Logger { get; }

    [UnitOfWork(true)]
    public async Task SeedAsync(Guid? tenantId = null)
    {
        const string defaultMenuGroup = "default";


        var dashboard = new Menu(GuidGenerator.Create(), null, "Dashboard", tenantId, true);
        dashboard.SetOrder(1);
        dashboard.SetGroupName(defaultMenuGroup);
        dashboard.SetIcon("fa fa-home");
        dashboard.SetRouter("dashboard");
        ;
        dashboard.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Dashboard" },
            { "zh-Hans", "综合看板" },
            { "zh-Hant", "綜合看板" }
        });

        await MenuManager.CreateAsync(dashboard);

        var systemsMaintenance =
            new Menu(GuidGenerator.Create(), null, "Systems Maintenance", tenantId, true);
        systemsMaintenance.SetGroupName(defaultMenuGroup);
        systemsMaintenance.SetOrder(100);
        systemsMaintenance.SetIcon("fa fa-cogs");
        systemsMaintenance.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Systems Maintenance" },
            { "zh-Hans", "系统维护" },
            { "zh-Hant", "系統維護" }
        });


        await MenuManager.CreateAsync(systemsMaintenance);

        var identityManagement = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "Identity Management",
            tenantId, true);
        identityManagement.SetGroupName(defaultMenuGroup);
        identityManagement.SetOrder(101);
        identityManagement.SetIcon("fa fa-id-card");
        identityManagement.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Identity Management" },
            { "zh-Hans", "身份管理" },
            { "zh-Hant", "身分管理" }
        });

        await MenuManager.CreateAsync(identityManagement);

        var users = new Menu(GuidGenerator.Create(), identityManagement.Id, "Users", tenantId, true);
        users.SetGroupName(defaultMenuGroup);
        users.SetOrder(102);
        users.SetIcon("fa fa-users");
        users.SetRouter("users");
        users.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Users" },
            { "zh-Hans", "用户" },
            { "zh-Hant", "用戶" }
        });
        users.SetPermissions(["AbpIdentity.Users"]);


        await MenuManager.CreateAsync(users);


        var roles = new Menu(GuidGenerator.Create(), identityManagement.Id, "Roles", tenantId, true);
        roles.SetGroupName(defaultMenuGroup);
        roles.SetOrder(104);
        roles.SetIcon("fa fa-user-tag");
        roles.SetRouter("roles");
        roles.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Roles" },
            { "zh-Hans", "角色" },
            { "zh-Hant", "角色" }
        });
        roles.SetPermissions(["AbpIdentity.Roles"]);
        await MenuManager.CreateAsync(roles);

        var securityLogManagement = new Menu(GuidGenerator.Create(), identityManagement.Id, "Security Logs",
            tenantId, true);
        securityLogManagement.SetGroupName(defaultMenuGroup);
        securityLogManagement.SetOrder(105);
        securityLogManagement.SetIcon("user-shield");
        securityLogManagement.SetRouter("security-logs");
        securityLogManagement.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Security Logs" },
            { "zh-Hans", "安全日志" },
            { "zh-Hant", "安全日誌" }
        });
        securityLogManagement.SetPermissions(["AbpIdentity.SecurityLog"]);

        if (!tenantId.HasValue)
        {
            var openIdDictManagement = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "OpenId Dict",
                isStatic: true);
            openIdDictManagement.SetGroupName(defaultMenuGroup);
            openIdDictManagement.SetOrder(106);
            openIdDictManagement.SetIcon("fa fa-shield");
            openIdDictManagement.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Authorization Grant" },
                { "zh-Hans", "授权管理" },
                { "zh-Hant", "授權管理" }
            });

            await MenuManager.CreateAsync(openIdDictManagement);

            var applications = new Menu(GuidGenerator.Create(), openIdDictManagement.Id, "Applications",
                isStatic: true);
            applications.SetGroupName(defaultMenuGroup);
            applications.SetOrder(107);
            applications.SetIcon("fa fa-anchor-lock");
            applications.SetRouter("applications");
            applications.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Applications" },
                { "zh-Hans", "应用" },
                { "zh-Hant", "應用" }
            });
            applications.SetPermissions(["OpenIddict.Applications"]);

            await MenuManager.CreateAsync(applications);

            var scopes = new Menu(GuidGenerator.Create(), openIdDictManagement.Id, "Scopes", isStatic: true);
            scopes.SetGroupName(defaultMenuGroup);
            scopes.SetOrder(108);
            scopes.SetRouter("scopes");
            scopes.SetIcon("fa fa-anchor");
            scopes.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Scopes" },
                { "zh-Hans", "授权范围" },
                { "zh-Hant", "授權範圍" }
            });

            scopes.SetPermissions(["OpenIddict.Scopes"]);
            await MenuManager.CreateAsync(scopes);

            var tenants = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "Tenants", isStatic: true);
            tenants.SetGroupName(defaultMenuGroup);
            tenants.SetOrder(109);
            tenants.SetIcon("fa fa-building-user");
            tenants.SetRouter("tenants");
            tenants.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Tenants" },
                { "zh-Hans", "租户" },
                { "zh-Hant", "租戶" }
            });

            tenants.SetPermissions(["AbpTenantManagement.Tenants"]);

            await MenuManager.CreateAsync(tenants);

            var settings = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "Settings",
                isStatic: true);
            settings.SetGroupName(defaultMenuGroup);
            settings.SetOrder(110);
            settings.SetIcon("fa fa-cog");
            settings.SetRouter("settings");
            settings.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Settings" },
                { "zh-Hans", "系统设置" },
                { "zh-Hant", "系統設置" }
            });

            await MenuManager.CreateAsync(settings);

            var auditLogs = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "Audit Logs",
                isStatic: true);
            auditLogs.SetGroupName(defaultMenuGroup);
            auditLogs.SetOrder(111);
            auditLogs.SetIcon("fa fa-file-lines");
            auditLogs.SetRouter("audit-logs");
            auditLogs.SetMultiLingual(new Dictionary<string, object>()
            {
                { "en", "Audit Logs" },
                { "zh-Hans", "审计日志" },
                { "zh-Hant", "稽核日誌" }
            });

            auditLogs.SetPermissions(["AuditLogging.AuditLogs"]);

            await MenuManager.CreateAsync(auditLogs);
        }

        var menus = new Menu(GuidGenerator.Create(), systemsMaintenance.Id, "Menus", tenantId, true);
        menus.SetGroupName(defaultMenuGroup);
        menus.SetOrder(111);
        menus.SetIcon("fa fa-list");
        menus.SetRouter("menus");
        menus.SetMultiLingual(new Dictionary<string, object>()
        {
            { "en", "Menus" },
            { "zh-Hans", "菜单" },
            { "zh-Hant", "菜單" }
        });
        menus.SetPermissions(["MenuManagement.Menus"]);
        await MenuManager.CreateAsync(menus);
    }
}