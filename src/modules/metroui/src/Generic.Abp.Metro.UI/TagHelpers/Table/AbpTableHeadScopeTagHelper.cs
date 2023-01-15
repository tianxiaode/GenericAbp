using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Table;

[HtmlTargetElement("th")]
public class AbpTableHeadScopeTagHelper : AbpTagHelper<AbpTableHeadScopeTagHelper, AbpTableHeadScopeTagHelperService>
{
    public AbpThScope Scope { get; set; } = AbpThScope.Default;

    public AbpTableHeadScopeTagHelper(AbpTableHeadScopeTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
