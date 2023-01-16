using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpStyleBundleTagHelper : AbpBundleTagHelper<AbpStyleBundleTagHelper, AbpStyleBundleTagHelperService>, IBundleTagHelper
{
    [HtmlAttributeName("preload")]
    public bool Preload { get; set; }

    public AbpStyleBundleTagHelper(AbpStyleBundleTagHelperService service)
        : base(service)
    {
    }
}
