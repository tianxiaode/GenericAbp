using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Blockquote;

[HtmlTargetElement("footer", ParentTag = "blockquote")]
public class AbpBlockquoteFooterTagHelper : AbpTagHelper<AbpBlockquoteFooterTagHelper, AbpBlockquoteFooterTagHelperService>
{
    public AbpBlockquoteFooterTagHelper(AbpBlockquoteFooterTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
