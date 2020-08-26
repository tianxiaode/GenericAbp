using Generic.Abp.Themes.Shared.PageToolbars;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Shared.Pages.Shared.Components.AbpPageToolbar
{
    public class AbpPageToolbarViewComponent : AbpViewComponent
    {
        private readonly IPageToolbarManager _toolbarManager;

        public AbpPageToolbarViewComponent(IPageToolbarManager toolbarManager)
        {
            _toolbarManager = toolbarManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string pageName)
        {
            var items = await _toolbarManager.GetItemsAsync(pageName);
            return View("~/Pages/Shared/Components/AbpPageToolbar/Default.cshtml", items);
        }
    }
}
