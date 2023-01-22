using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogTitleTagHelperService : MetroTagHelperService<MetroDialogTitleTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("dialog-title");
        await AddItemToItemsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogTitleTagHelper));
    }


}
