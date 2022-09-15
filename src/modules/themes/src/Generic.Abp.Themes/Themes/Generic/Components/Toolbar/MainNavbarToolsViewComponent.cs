using Generic.Abp.Themes.Shared.Toolbars;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Themes.Generic.Components.Toolbar
{
    public class MainNavbarToolbarViewComponent : AbpViewComponent
    {
        private readonly IToolbarManager _toolbarManager;

        public MainNavbarToolbarViewComponent(IToolbarManager toolbarManager)
        {
            _toolbarManager = toolbarManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var toolbar = await _toolbarManager.GetAsync(StandardToolbars.Main);
            return View("~/Themes/Generic/Components/Toolbar/Default.cshtml", toolbar);
        }
    }
}
