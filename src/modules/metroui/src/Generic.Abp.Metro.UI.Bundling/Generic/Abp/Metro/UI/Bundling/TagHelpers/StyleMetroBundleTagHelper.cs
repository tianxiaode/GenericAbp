using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class StyleMetroBundleTagHelper : MetroBundleTagHelper<StyleMetroBundleTagHelper, StyleMetroBundleTagHelperService>, IBundleTagHelper
{
    [HtmlAttributeName("preload")]
    public bool Preload { get; set; }

    public StyleMetroBundleTagHelper(StyleMetroBundleTagHelperService service)
        : base(service)
    {
    }
}
