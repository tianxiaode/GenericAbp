﻿using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;

namespace Generic.Abp.Themes.Themes.Generic.Components.Toolbar.LanguageSwitch
{
    public class LanguageSwitchViewComponent : AbpViewComponent
    {
        private readonly ILanguageProvider _languageProvider;

        public LanguageSwitchViewComponent(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageProvider.GetLanguagesAsync();
            var currentLanguage = languages.FindByCulture(
                CultureInfo.CurrentCulture.Name,
                CultureInfo.CurrentUICulture.Name
            );

            var model = new LanguageSwitchViewComponentModel
            {
                CurrentLanguage = currentLanguage,
                OtherLanguages = languages.Where(l => l != currentLanguage).ToList()
            };

            return View("~/Themes/Generic/Components/Toolbar/LanguageSwitch/Default.cshtml", model);
        }
    }
}
