using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Figure;

public class AbpFigureCaptionTagHelperService : AbpTagHelperService<AbpFigureCaptionTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "figcaption";
        output.Attributes.AddClass("figure-caption");
    }
}
