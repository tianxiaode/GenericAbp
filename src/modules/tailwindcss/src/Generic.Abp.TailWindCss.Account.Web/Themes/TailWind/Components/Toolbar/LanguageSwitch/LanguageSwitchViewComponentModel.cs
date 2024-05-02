using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Generic.Abp.TailWindCss.Account.Web.Themes.TailWind.Components.Toolbar.LanguageSwitch;

public class LanguageSwitchViewComponentModel
{
    public LanguageInfo CurrentLanguage { get; set; }

    public List<LanguageInfo> OtherLanguages { get; set; }
}