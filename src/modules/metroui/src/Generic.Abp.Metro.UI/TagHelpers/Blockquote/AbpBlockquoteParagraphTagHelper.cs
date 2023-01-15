using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Blockquote;

[HtmlTargetElement("p", ParentTag = "blockquote")]
public class AbpBlockquoteParagraphTagHelper : AbpTagHelper<AbpBlockquoteParagraphTagHelper, AbpBlockquoteParagraphTagHelperService>
{
    public AbpBlockquoteParagraphTagHelper(AbpBlockquoteParagraphTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
