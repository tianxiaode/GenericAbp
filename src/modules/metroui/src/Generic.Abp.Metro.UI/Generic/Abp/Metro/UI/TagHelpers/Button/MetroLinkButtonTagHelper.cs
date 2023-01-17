using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

[HtmlTargetElement("a", Attributes = "metro-button", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("input", Attributes = "metro-button", TagStructure = TagStructure.WithoutEndTag)]
public class MetroLinkButtonTagHelper : MetroTagHelper<MetroLinkButtonTagHelper, MetroLinkButtonTagHelperService>, IButtonTagHelperBase
{
    [HtmlAttributeName("metro-button")]
    public MetroButtonType ButtonType { get; set; }

    public MetroButtonSize Size { get; set; } = MetroButtonSize.Default;

    public string Text { get; set; }

    public string Icon { get; set; }

    public bool? Disabled { get; set; }

    public FontIconType IconType { get; } = FontIconType.FontAwesome;

    public MetroLinkButtonTagHelper(MetroLinkButtonTagHelperService service)
        : base(service)
    {

    }
}
