using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroScriptTagHelper : MetroBundleItemTagHelper<MetroScriptTagHelper, MetroScriptTagHelperService>, IBundleItemTagHelper
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    public MetroScriptTagHelper(MetroScriptTagHelperService service)
        : base(service)
    {

    }

    protected override string GetFileExtension()
    {
        return "js";
    }
}
