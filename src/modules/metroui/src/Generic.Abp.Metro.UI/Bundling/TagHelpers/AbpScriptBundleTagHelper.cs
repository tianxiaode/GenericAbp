using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpScriptBundleTagHelper : AbpBundleTagHelper<AbpScriptBundleTagHelper, AbpScriptBundleTagHelperService>, IBundleTagHelper
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    public AbpScriptBundleTagHelper(AbpScriptBundleTagHelperService service)
        : base(service)
    {

    }
}
