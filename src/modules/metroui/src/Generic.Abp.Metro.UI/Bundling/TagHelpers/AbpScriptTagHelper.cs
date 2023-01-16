using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpScriptTagHelper : AbpBundleItemTagHelper<AbpScriptTagHelper, AbpScriptTagHelperService>, IBundleItemTagHelper
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    public AbpScriptTagHelper(AbpScriptTagHelperService service)
        : base(service)
    {

    }

    protected override string GetFileExtension()
    {
        return "js";
    }
}
