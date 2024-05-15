using System;
using System.Collections.Generic;
using Generic.Abp.Tailwind.Providers;
using QuickTemplate.Localization;

namespace QuickTemplate.Web.Providers;

public class QuicktemplateLayoutMenuProvider : DefaultLayoutMenuProvider
{
    public QuicktemplateLayoutMenuProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Type ResourceType { get; set; } = typeof(QuickTemplateResource);

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