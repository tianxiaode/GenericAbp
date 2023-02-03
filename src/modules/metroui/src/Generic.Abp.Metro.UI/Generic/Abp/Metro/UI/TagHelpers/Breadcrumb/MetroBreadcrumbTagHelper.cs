using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public class MetroBreadcrumbTagHelper : MetroTagHelper<BreadcrumbGroupItem>
{
    public MetroBreadcrumbTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
    }

    public override void Init(TagHelperContext context)
    {
        GroupItemsName = TagHelperConsts.BreadcrumbItems;
        InitGroupItems(context);
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("breadcrumbs");
        var child = await output.GetChildContentAsync();
        await SetItemsAsync(context, output);
    }

    protected virtual async Task SetItemsAsync(TagHelperContext context, TagHelperOutput output)
    {
        var list = await GetGroupItems(context);
        var builder = new StringBuilder();
        foreach (var item in list)
        {
            builder.AppendLine(item.HtmlContent);
        }

        output.Content.AppendHtml(builder.ToString());
    }
}