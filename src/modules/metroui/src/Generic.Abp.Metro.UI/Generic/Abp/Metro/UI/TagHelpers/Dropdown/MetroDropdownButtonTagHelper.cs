using Generic.Abp.Metro.UI.TagHelpers.Button;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownButtonTagHelper : MetroTagHelper<MetroDropdownButtonTagHelper, MetroDropdownButtonTagHelperService>
{
    public string Text { get; set; }

    public MetroButtonSize Size { get; set; } = MetroButtonSize.Default;

    public DropdownStyle DropdownStyle { get; set; } = DropdownStyle.Single;

    public MetroButtonType ButtonType { get; set; } = MetroButtonType.Default;

    public string Icon { get; set; }

    public FontIconType IconType { get; set; } = FontIconType.FontAwesome;

    public bool? Link { get; set; }

    public bool? NavLink { get; set; }

    public MetroDropdownButtonTagHelper(MetroDropdownButtonTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
