using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Border;

[HtmlTargetElement(Attributes = "abp-border")]
public class AbpBorderTagHelper : AbpTagHelper<AbpBorderTagHelper, AbpBorderTagHelperService>
{
    public AbpBorderType AbpBorder { get; set; } = AbpBorderType.Default;

    public AbpBorderTagHelper(AbpBorderTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
