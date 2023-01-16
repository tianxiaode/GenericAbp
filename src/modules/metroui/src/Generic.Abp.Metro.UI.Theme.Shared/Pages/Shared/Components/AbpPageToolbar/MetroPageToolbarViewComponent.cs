using Generic.Abp.Metro.UI.Theme.Shared.PageToolbars;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar;

public class MetroPageToolbarViewComponent : AbpViewComponent
{
    protected IPageToolbarManager ToolbarManager { get; }

    public MetroPageToolbarViewComponent(IPageToolbarManager toolbarManager)
    {
        ToolbarManager = toolbarManager;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string pageName)
    {
        var items = await ToolbarManager.GetItemsAsync(pageName);
        return View("~/Pages/Shared/Components/AbpPageToolbar/Default.cshtml", items);
    }
}
