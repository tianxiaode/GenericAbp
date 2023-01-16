using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Theme.Shared.Toolbars;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Theme.Basic.Themes.Basic.Components.Toolbar;

public class MainNavbarToolbarViewComponent : AbpViewComponent
{
    protected IToolbarManager ToolbarManager { get; }

    public MainNavbarToolbarViewComponent(IToolbarManager toolbarManager)
    {
        ToolbarManager = toolbarManager;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);
        return View("~/Themes/Basic/Components/Toolbar/Default.cshtml", toolbar);
    }
}
