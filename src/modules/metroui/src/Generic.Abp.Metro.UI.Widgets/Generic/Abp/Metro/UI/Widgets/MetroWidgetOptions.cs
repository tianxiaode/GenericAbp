namespace Generic.Abp.Metro.UI.Widgets;

public class MetroWidgetOptions
{
    public WidgetDefinitionCollection Widgets { get; }

    public MetroWidgetOptions()
    {
        Widgets = new WidgetDefinitionCollection();
    }
}
