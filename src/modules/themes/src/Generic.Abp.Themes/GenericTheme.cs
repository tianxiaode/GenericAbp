using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Themes
{
    [ThemeName(Name)]
    public class GenericTheme : ITheme, ITransientDependency
    {
        public const string Name = "Generic";

        public virtual string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Application:
                    return "~/Themes/Generic/Layouts/Application.cshtml";
                case StandardLayouts.Account:
                    return "~/Themes/Generic/Layouts/Account.cshtml";
                case StandardLayouts.Empty:
                    return "~/Themes/Generic/Layouts/Empty.cshtml";
                default:
                    return fallbackToDefault ? "~/Themes/Generic/Layouts/Application.cshtml" : null;
            }
        }
    }
}
