using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Border;

[HtmlTargetElement(Attributes = "abp-rounded")]
public class AbpRoundedTagHelper : AbpTagHelper<AbpRoundedTagHelper, AbpRoundedTagHelperService>
{
    public AbpRoundedType AbpRounded { get; set; } = AbpRoundedType.Default;

    public AbpRoundedTagHelper(AbpRoundedTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
