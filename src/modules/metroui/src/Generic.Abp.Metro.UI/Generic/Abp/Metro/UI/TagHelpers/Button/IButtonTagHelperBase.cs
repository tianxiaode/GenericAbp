namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public interface IButtonTagHelperBase
{
    MetroColor Color { get; }
    MetroButtonSize Size { get; }
    public ButtonShape Shape { get; }
    ButtonStyle ButtonStyle { get; }
    string Text { get; }
    string Icon { get; }
    bool? Disabled { get; }
    FontIconType IconType { get; }
    public bool? Outline { get; }
    public bool? Rounded { get; }
    public bool? Shadowed { get; }
    string Caption { get; }
    bool? IconRight { get; }
}