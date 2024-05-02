using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.TailWindCss.Account.Web;

[ThemeName(Name)]
public class TailWindTheme : ITheme, ITransientDependency
{
    public const string Name = "TailWind";

    public virtual string GetLayout(string name, bool fallbackToDefault = true)
    {
        return "~/Themes/TailWind/Layouts/Empty.cshtml";
    }
}