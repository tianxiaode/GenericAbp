using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogContentTagHelperService : MetroTagHelperService<MetroDialogContentTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var attributes = output.Attributes;
        attributes.AddClass("dialog-content");
        attributes.AddClass("flex-fill");
        attributes.AddClass("scroll-y");
        await AddItemToItemsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogContentTagHelper));
    }
}
