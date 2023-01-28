using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroScriptBundleTagHelper : MetroBundleTagHelper, IBundleTagHelper
{
    [HtmlAttributeName("defer")] public bool Defer { get; set; }


    public MetroScriptBundleTagHelper(MetroTagHelperScriptService resourceService) : base(
        resourceService)
    {
    }
}