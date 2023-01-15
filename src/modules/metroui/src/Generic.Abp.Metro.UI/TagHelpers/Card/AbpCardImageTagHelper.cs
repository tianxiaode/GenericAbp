using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("img", Attributes = "abp-card-image", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement("abp-image", Attributes = "abp-card-image", TagStructure = TagStructure.WithoutEndTag)]
public class AbpCardImageTagHelper : AbpTagHelper<AbpCardImageTagHelper, AbpCardImageTagHelperService>
{
    [HtmlAttributeName("abp-card-image")]
    public AbpCardImagePosition Position { get; set; } = AbpCardImagePosition.Top;

    public AbpCardImageTagHelper(AbpCardImageTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
