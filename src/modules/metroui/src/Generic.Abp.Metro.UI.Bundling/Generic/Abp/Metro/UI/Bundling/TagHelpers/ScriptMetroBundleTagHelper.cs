using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("abp-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ScriptMetroBundleTagHelper : MetroBundleTagHelper<ScriptMetroBundleTagHelper, ScriptMetroBundleTagHelperService>, IBundleTagHelper
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    public ScriptMetroBundleTagHelper(ScriptMetroBundleTagHelperService service)
        : base(service)
    {

    }
}
