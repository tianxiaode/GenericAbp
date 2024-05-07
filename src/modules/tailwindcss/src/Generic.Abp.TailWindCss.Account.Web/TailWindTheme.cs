using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.TailWindCss.Account.Web;

[ThemeName(Name)]
public class TailWindTheme : ITheme, ITransientDependency
{
    public const string Name = "TailWind";

    public virtual string GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Account => "~/Pages/Shared/_AccountLayout.cshtml",
            StandardLayouts.Application => "~/Pages/Shared/_Layout.cshtml",
            _ => fallbackToDefault ? "~/Pages/Shared/_EmptyLayout.cshtml" : null
        } ?? string.Empty;
    }
}