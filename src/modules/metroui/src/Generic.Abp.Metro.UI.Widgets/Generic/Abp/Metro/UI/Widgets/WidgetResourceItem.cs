using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Generic.Abp.Metro.UI.Widgets;

public class WidgetResourceItem
{
    public string Src { get; }

    public Type Type { get; }

    public WidgetResourceItem(string src)
    {
        Src = Check.NotNullOrWhiteSpace(src, nameof(src));
    }

    public WidgetResourceItem(Type type)
    {
        Type = Check.NotNull(type, nameof(type));
    }
}
