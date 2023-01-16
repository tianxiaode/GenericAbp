using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.Widgets;

public interface IPageWidgetManager
{
    bool TryAdd(WidgetDefinition widget);

    IReadOnlyList<WidgetDefinition> GetAll();
}
