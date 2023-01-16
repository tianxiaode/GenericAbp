using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Widgets.Components.WidgetScripts;

public class WidgetScriptsViewComponent : AbpViewComponent
{
    protected IPageWidgetManager PageWidgetManager { get; }
    protected MetroWidgetOptions Options { get; }

    public WidgetScriptsViewComponent(
        IPageWidgetManager pageWidgetManager,
        IOptions<MetroWidgetOptions> options)
    {
        PageWidgetManager = pageWidgetManager;
        Options = options.Value;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View(
            "~/Generic/Abp/Metro/UI/Widgets/Components/WidgetScripts/Default.cshtml",
            new WidgetResourcesViewModel
            {
                Widgets = PageWidgetManager.GetAll()
            }
        );
    }
}
