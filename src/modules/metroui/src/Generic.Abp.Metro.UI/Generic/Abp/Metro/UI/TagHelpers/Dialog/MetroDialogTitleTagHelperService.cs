using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogTitleTagHelperService : MetroTagHelperService<MetroDialogTitleTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        AddClasses(context, output);
        output.Content.AppendHtml(TagHelper.Title);
    }

    protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("dialog-title");
    }


}
