﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace Generic.Abp.Themes.Themes.Generic.Components.Menu
{
    public class MainNavbarMenuViewComponent : AbpViewComponent
    {
        private readonly IMenuManager _menuManager;

        public MainNavbarMenuViewComponent(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = await _menuManager.GetAsync(StandardMenus.Main);
            return View("~/Themes/Generic/Components/Menu/Default.cshtml", menu);
        }
    }
}
