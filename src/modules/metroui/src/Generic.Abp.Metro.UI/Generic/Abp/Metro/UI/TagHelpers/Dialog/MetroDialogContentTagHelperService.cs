using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogContentTagHelperService : MetroTagHelperService<MetroDialogContentTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("dialog-content");
    }
}
