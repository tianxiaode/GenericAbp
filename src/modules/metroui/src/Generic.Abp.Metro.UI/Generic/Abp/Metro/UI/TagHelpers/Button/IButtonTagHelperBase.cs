namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public interface IButtonTagHelperBase
{
    MetroButtonType ButtonType { get; }
    MetroButtonSize Size { get; }
    public ButtonShape Shape { get; set; }
    string Text { get; }
    string Icon { get; }
    bool? Disabled { get; }
    FontIconType IconType { get; }
    public bool? Outline { get; }
    public bool? Rounded { get; }
    public bool? Shadowed { get; }
}