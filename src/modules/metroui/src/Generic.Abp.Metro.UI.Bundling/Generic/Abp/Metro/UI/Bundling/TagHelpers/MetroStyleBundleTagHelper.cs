using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroStyleBundleTagHelper : MetroBundleTagHelper, IBundleTagHelper
{
    public MetroStyleBundleTagHelper(MetroTagHelperStyleService resourceService) : base(
        resourceService)
    {
    }

    [HtmlAttributeName("preload")] public bool Preload { get; set; }
}