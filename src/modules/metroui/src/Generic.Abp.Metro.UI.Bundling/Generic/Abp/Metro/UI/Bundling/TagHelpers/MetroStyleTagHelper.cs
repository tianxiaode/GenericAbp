using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("asp-style", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroStyleTagHelper : MetroBundleItemTagHelper<MetroStyleTagHelper, MetroStyleTagHelperService>, IBundleItemTagHelper
{
    [HtmlAttributeName("preload")]
    public bool Preload { get; set; }

    public MetroStyleTagHelper(MetroStyleTagHelperService service)
        : base(service)
    {

    }

    protected override string GetFileExtension()
    {
        return "css";
    }
}
