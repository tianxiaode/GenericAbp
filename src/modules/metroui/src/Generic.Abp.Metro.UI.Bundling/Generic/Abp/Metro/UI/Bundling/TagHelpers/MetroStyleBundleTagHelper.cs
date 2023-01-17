using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroStyleBundleTagHelper : MetroBundleTagHelper<MetroStyleBundleTagHelper, MetroStyleBundleTagHelperService>, IBundleTagHelper
{
    [HtmlAttributeName("preload")]
    public bool Preload { get; set; }

    public MetroStyleBundleTagHelper(MetroStyleBundleTagHelperService service)
        : base(service)
    {
    }
}
