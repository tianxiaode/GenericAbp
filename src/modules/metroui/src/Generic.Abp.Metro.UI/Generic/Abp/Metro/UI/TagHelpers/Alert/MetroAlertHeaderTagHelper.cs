using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Alert;

[HtmlTargetElement("h1", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("h2", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("h3", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("h4", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("h5", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("h6", ParentTag = "metro-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroAlertHeaderTagHelper : MetroTagHelper<MetroAlertHeaderTagHelper, MetroAlertHeaderTagHelperService>
{
    public MetroAlertHeaderTagHelper(MetroAlertHeaderTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
