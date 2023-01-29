using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public class MetroBreadcrumbItemTagHelper : MetroTagHelper<BreadcrumbGroupItem>, IBreadcrumbItem
{
    public string Title { get; set; }
    public string Cls { get; set; }
    public string Url { get; set; }
    public int DisplayOrder { get; set; } = 0;

    public override void Init(TagHelperContext context)
    {
        GroupItemsName = TagHelperConsts.BreadcrumbItems;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "li";
        output.Attributes.AddClass("page-item");
        DisplayOrder = await GetDisplayOrderAsync(DisplayOrder);
        var html =
            $"<li class=\"page-item\" style=\"order:{DisplayOrder}\"><a href=\"{Url}\" class=\"page-link {Cls}\">{Title}</a></li>";
        await AddGroupItemAsync(context, Title, DisplayOrder, html);
        output.SuppressOutput();
    }
}