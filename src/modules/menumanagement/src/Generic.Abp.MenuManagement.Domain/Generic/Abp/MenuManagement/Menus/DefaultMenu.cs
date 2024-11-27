using System;
using System.Collections.Generic;

namespace Generic.Abp.MenuManagement.Menus;

public class DefaultMenu
{
    public const string ContentManagement = "ContentManagement";
    public const string Tenants = "Tenants";

    public static List<MenuConfig> GetMenuConfigurations()
    {
        return
        [
            new MenuConfig("Dashboard", "fa fa-home", "dashboard", new Dictionary<string, object>
            {
                { "en", "Dashboard" },
                { "zh-Hans", "仪表盘" },
                { "zh-Hant", "儀表板" }
            }, [], 1, []),
            new MenuConfig(ContentManagement, "fa fa-book", null,
                new Dictionary<string, object>
                {
                    { "en", "Content Management" },
                    { "zh-Hans", "内容管理" },
                    { "zh-Hant", "內容管理" }
                }, [], 100,
                []),
            new MenuConfig("System Management", "fa fa-cogs", null,
                new Dictionary<string, object>
                {
                    { "en", "System Management" },
                    { "zh-Hans", "系统管理" },
                    { "zh-Hant", "系統管理" }
                }, [], 200, [
                    new MenuConfig("Settings", "fa fa-cog", "settings",
                        new Dictionary<string, object>
                        {
                            { "en", "Settings" },
                            { "zh-Hans", "系统设置" },
                            { "zh-Hant", "系統設置" }
                        }, [], 210, []),
                    new MenuConfig("Audit Logs", "fa fa-file-lines", "audit-logs",
                        new Dictionary<string, object>
                        {
                            { "en", "Audit Logs" },
                            { "zh-Hans", "审计日志" },
                            { "zh-Hant", "稽核日誌" }
                        }, [], 220, []),
                    new MenuConfig(Tenants, "fa fa-building-user", "tenants",
                        new Dictionary<string, object>
                        {
                            { "en", "Tenants" },
                            { "zh-Hans", "租户" },
                            { "zh-Hant", "租戶" }
                        }, ["AbpTenantManagement.Tenants"], 230, []),
                    new MenuConfig("Menus", "fa fa-list", "menus",
                        new Dictionary<string, object>
                        {
                            { "en", "Menus" },
                            { "zh-Hans", "菜单" },
                            { "zh-Hant", "菜單" }
                        }, ["MenuManagement.Menus"], 240, []),
                ]),
            new MenuConfig("User Management", "fa fa-id-card", null,
                new Dictionary<string, object>
                {
                    { "en", "User Management" },
                    { "zh-Hans", "用户管理" },
                    { "zh-Hant", "用戶管理" }
                }, [], 300, [
                    new MenuConfig("Users", "fa fa-users", "users",
                        new Dictionary<string, object>
                        {
                            { "en", "Users" },
                            { "zh-Hans", "用户" },
                            { "zh-Hant", "用戶" }
                        }, ["AbpIdentity.Users"], 310, []
                    ),
                    new MenuConfig("Roles", "fa fa-user-tag", "roles",
                        new Dictionary<string, object>
                        {
                            { "en", "Roles" },
                            { "zh-Hans", "角色" },
                            { "zh-Hant", "角色" }
                        }, ["AbpIdentity.Roles"], 320, []
                    ),
                    new MenuConfig("Security Logs", "fa fa-user-shield", "security-logs",
                        new Dictionary<string, object>
                        {
                            { "en", "Security Logs" },
                            { "zh-Hans", "安全日志" },
                            { "zh-Hant", "安全日誌" }
                        }, ["AbpIdentity.SecurityLogs"], 330, []
                    ),
                ]
            ),
            new MenuConfig("Security & Authentication", "fa fa-shield", null,
                new Dictionary<string, object>
                {
                    { "en", "Security & Authentication" },
                    { "zh-Hans", "安全与认证" },
                    { "zh-Hant", "安全與認證" }
                }, [], 400, [
                    new MenuConfig("Applications", "fa fa-anchor-lock", "applications",
                        new Dictionary<string, object>
                        {
                            { "en", "Applications" },
                            { "zh-Hans", "应用程序" },
                            { "zh-Hant", "應用程式" }
                        }, ["OpenIddict.Applications"], 410, []
                    ),
                    new MenuConfig("Scopes", "fa fa-anchor", "scopes",
                        new Dictionary<string, object>
                        {
                            { "en", "Scopes" },
                            { "zh-Hans", "授权范围" },
                            { "zh-Hant", "授權範圍" }
                        }, ["OpenIddict.Scopes"], 420, []
                    ),
                ]
            )
        ];
    }
}

public class MenuConfig(
    string name,
    string icon,
    string? router,
    Dictionary<string, object> translations,
    List<string> permissions,
    int order,
    List<MenuConfig> items)
{
    public string Name { get; set; } = name;
    public string Icon { get; set; } = icon;
    public string? Router { get; set; } = router;
    public Dictionary<string, object> Translations { get; set; } = translations;
    public List<string> Permissions { get; set; } = permissions;
    public int Order { get; set; } = order;
    public List<MenuConfig> Items { get; set; } = items;
}