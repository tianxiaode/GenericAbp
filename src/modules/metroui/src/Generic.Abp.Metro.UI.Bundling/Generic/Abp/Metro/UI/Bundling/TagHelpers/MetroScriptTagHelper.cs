using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroScriptTagHelper : MetroBundleItemTagHelper, IBundleItemTagHelper
{
    public MetroScriptTagHelper(MetroTagHelperScriptService resourceService) : base(
        resourceService)
    {
    }

    [HtmlAttributeName("defer")] public bool Defer { get; set; }

    protected override string GetFileExtension()
    {
        return "js";
    }
}