using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-style", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroStyleTagHelper : MetroBundleItemTagHelper, IBundleItemTagHelper
{
    public MetroStyleTagHelper(MetroTagHelperStyleService resourceService) : base(
        resourceService)
    {
    }

    [HtmlAttributeName("preload")] public bool Preload { get; set; }


    protected override string GetFileExtension()
    {
        return "css";
    }
}