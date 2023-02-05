using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class MetroCardContentTagHelper : MetroCardTagHelperBase
{
    public MetroCardContentTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
        GroupItemsName = TagHelperConsts.CardGroupItems;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("card-content");
        var child = await output.GetChildContentAsync();
        output.Content.AppendHtml(child.GetContent());
        await AddGroupItemAsync(context, nameof(MetroCardContentTagHelper), await output.RenderAsync(HtmlEncoder));
        output.SuppressOutput();
    }
}