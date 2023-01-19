namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public interface IButtonTagHelperBase
{
    MetroButtonType ButtonType { get; }

    MetroButtonSize Size { get; }

    string Text { get; }

    string Icon { get; }

    bool? Disabled { get; }

    FontIconType IconType { get; }
}
