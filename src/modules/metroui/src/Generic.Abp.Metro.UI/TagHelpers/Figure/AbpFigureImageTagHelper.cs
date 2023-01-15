using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Figure;

[HtmlTargetElement("abp-image", ParentTag = "abp-figure")]
public class AbpFigureImageTagHelper : AbpTagHelper<AbpFigureImageTagHelper, AbpFigureImageTagHelperService>
{
    public AbpFigureImageTagHelper(AbpFigureImageTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
