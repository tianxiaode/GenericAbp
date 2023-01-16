using System;
using System.Collections.Generic;
using Generic.Abp.Metro.UI.Bundling;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.Widgets;

[DependsOn(
    typeof(GenericAbpMetroUiModule),
    typeof(GenericAbpMetroUiBundlingModule)
)]
public class GenericAbpMetroUiWidgetsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiWidgetsModule).Assembly);
        });

        AutoAddWidgets(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<DefaultViewComponentHelper>();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiWidgetsModule>();
        });
    }

    private static void AutoAddWidgets(IServiceCollection services)
    {
        var widgetTypes = new List<Type>();

        services.OnRegistred(context =>
        {
            if (WidgetAttribute.IsWidget(context.ImplementationType))
            {
                widgetTypes.Add(context.ImplementationType);
            }
        });

        services.Configure<MetroWidgetOptions>(options =>
        {
            foreach (var widgetType in widgetTypes)
            {
                options.Widgets.Add(new WidgetDefinition(widgetType));
            }
        });
    }
}
