using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

[HtmlTargetElement("metro-button", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroButtonTagHelper : MetroTagHelper<MetroButtonTagHelper, MetroButtonTagHelperService>, IButtonTagHelperBase
{
    public MetroButtonType ButtonType { get; set; } 

    public MetroButtonSize Size { get; set; } 

    public string BusyText { get; set; }

    public string Text { get; set; }

    public string Icon { get; set; }
    public bool? Disabled { get; set; }

    public FontIconType IconType { get; set; } = FontIconType.Metro;

    public bool BusyTextIsHtml { get; set; }

    public MetroButtonTagHelper(MetroButtonTagHelperService service)
        : base(service)
    {

    }
}

