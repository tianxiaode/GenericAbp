using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Grid;

[HtmlTargetElement("abp-row")]
[HtmlTargetElement("abp-form-row")]
public class AbpRowTagHelper : AbpTagHelper<AbpRowTagHelper, AbpRowTagHelperService>
{
    public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;

    public HorizontalAlign HAlign { get; set; } = HorizontalAlign.Default;

    public bool? Gutters { get; set; } = true;

    public AbpRowTagHelper(AbpRowTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
