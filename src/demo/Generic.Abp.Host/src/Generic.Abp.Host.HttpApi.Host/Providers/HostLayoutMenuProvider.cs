using System;
using System.Collections.Generic;
using Generic.Abp.Host.Localization;
using Generic.Abp.TailWindCss.Account.Web.Providers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Generic.Abp.Host.Providers;

public class HostLayoutMenuProvider : DefaultLayoutMenuProvider
{
    public HostLayoutMenuProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Type ResourceType { get; set; } = typeof(HostResource);

    protected override List<LayoutMenu> MainMenus { get; set; } =
    [
        new LayoutMenu("Home", "/"),
        new LayoutMenu("About", "/about")
    ];

    protected override List<LayoutMenu> MobileMenus { get; set; } =
    [
        new LayoutMenu("Home", "/"),
        new LayoutMenu("About", "/about")
    ];
}