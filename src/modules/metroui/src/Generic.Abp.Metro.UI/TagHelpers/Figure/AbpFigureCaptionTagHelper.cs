using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Figure;

[HtmlTargetElement("abp-figcaption")]
public class AbpFigureCaptionTagHelper : AbpTagHelper<AbpFigureCaptionTagHelper, AbpFigureCaptionTagHelperService>
{
    public AbpFigureCaptionTagHelper(AbpFigureCaptionTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
