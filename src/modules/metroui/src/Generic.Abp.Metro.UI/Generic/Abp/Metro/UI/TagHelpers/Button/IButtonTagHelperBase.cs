using Generic.Abp.Metro.UI.TagHelpers.Core;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public interface IButtonTagHelperBase
{
    MetroAccentColor AccentColor { get; }
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
    HintPosition HintPosition { get; set; }
    string HintText { get; set; }
    string ClsHint { get; set; }
}