using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Theme.Basic;

[ThemeName(Name)]
public class BasicTheme : ITheme, ITransientDependency
{
    public const string Name = "Basic";

    public virtual string GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => "~/Themes/Basic/Layouts/Application.cshtml",
            StandardLayouts.Account => "~/Themes/Basic/Layouts/Account.cshtml",
            StandardLayouts.Empty => "~/Themes/Basic/Layouts/Empty.cshtml",
            StandardLayouts.Public => "~/Themes/Basic/Layouts/Public.cshtml",
            _ => fallbackToDefault ? "~/Themes/Basic/Layouts/Application.cshtml" : null
        };
    }
}