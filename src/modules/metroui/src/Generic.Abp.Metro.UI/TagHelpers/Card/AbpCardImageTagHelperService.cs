using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardImageTagHelperService : AbpTagHelperService<AbpCardImageTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass(TagHelper.Position.ToClassName());
        output.Attributes.RemoveAll("abp-card-image");
    }
}
