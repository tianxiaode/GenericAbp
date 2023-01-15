using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("a", Attributes = "abp-card-link")]
public class AbpCardLinkTagHelper : AbpTagHelper<AbpCardLinkTagHelper, AbpCardLinkTagHelperService>
{
    public AbpCardLinkTagHelper(AbpCardLinkTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
