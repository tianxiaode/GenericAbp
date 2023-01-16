using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Widgets.Components.WidgetStyles;

public class WidgetStylesViewComponent : AbpViewComponent
{
    protected IPageWidgetManager PageWidgetManager { get; }
    protected MetroWidgetOptions Options { get; }

    public WidgetStylesViewComponent(
        IPageWidgetManager pageWidgetManager,
        IOptions<MetroWidgetOptions> options)
    {
        PageWidgetManager = pageWidgetManager;
        Options = options.Value;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View(
            "~/Generic/Abp/Metro/UI/Widgets/Components/WidgetStyles/Default.cshtml",
            new WidgetResourcesViewModel
            {
                Widgets = PageWidgetManager.GetAll()
            }
        );
    }
}
