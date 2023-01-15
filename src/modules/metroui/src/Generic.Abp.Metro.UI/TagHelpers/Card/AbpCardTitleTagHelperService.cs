using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardTitleTagHelperService : AbpTagHelperService<AbpCardTitleTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = TagHelper.Heading.ToHtmlTag();
        output.Attributes.AddClass("card-title");
    }
}
