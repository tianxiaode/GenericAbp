using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardBodyTagHelperService : AbpTagHelperService<AbpCardBodyTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("card-body");

        ProcessTitle(output);
        ProcessSubtitle(output);
    }

    protected virtual void ProcessTitle(TagHelperOutput output)
    {
        if (!TagHelper.Title.IsNullOrWhiteSpace())
        {
            var cardTitle = new TagBuilder(AbpCardTitleTagHelper.DefaultHeading.ToHtmlTag());
            cardTitle.AddCssClass("card-title");
            cardTitle.InnerHtml.AppendHtml(TagHelper.Title);
            output.PreContent.AppendHtml(cardTitle);
        }
    }

    protected virtual void ProcessSubtitle(TagHelperOutput output)
    {
        if (!TagHelper.Subtitle.IsNullOrWhiteSpace())
        {
            var cardSubtitle = new TagBuilder(AbpCardSubtitleTagHelper.DefaultHeading.ToHtmlTag());
            cardSubtitle.AddCssClass("card-subtitle text-muted mb-2");
            cardSubtitle.InnerHtml.AppendHtml(TagHelper.Subtitle);
            output.PreContent.AppendHtml(cardSubtitle);
        }
    }
}
