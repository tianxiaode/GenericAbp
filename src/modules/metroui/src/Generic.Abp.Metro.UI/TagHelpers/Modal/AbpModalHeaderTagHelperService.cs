using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

public class AbpModalHeaderTagHelperService : AbpTagHelperService<AbpModalHeaderTagHelper>
{
    protected IStringLocalizer<AbpUiResource> L { get; }

    public AbpModalHeaderTagHelperService(IStringLocalizer<AbpUiResource> localizer)
    {
        L = localizer;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("modal-header");
        output.PreContent.SetHtmlContent(CreatePreContent());
        output.PostContent.SetHtmlContent(CreatePostContent());
    }

    protected virtual string CreatePreContent()
    {
        var title = new TagBuilder("h5");
        title.AddCssClass("modal-title");
        title.InnerHtml.AppendHtml(TagHelper.Title);

        return title.ToHtmlString();
    }

    protected virtual string CreatePostContent()
    {
        var button = new TagBuilder("button");
        button.AddCssClass("btn-close");
        button.Attributes.Add("type", "button");
        button.Attributes.Add("data-bs-dismiss", "modal");
        button.Attributes.Add("aria-label", L["Close"].Value);

        return button.ToHtmlString();
    }
}
