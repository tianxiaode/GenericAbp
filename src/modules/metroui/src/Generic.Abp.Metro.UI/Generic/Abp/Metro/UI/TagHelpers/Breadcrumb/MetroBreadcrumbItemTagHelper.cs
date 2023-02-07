using Generic.Abp.Metro.UI.TagHelpers.Menu;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

[HtmlTargetElement("metro-breadcrumb-item", TagStructure = TagStructure.WithoutEndTag)]
public class MetroBreadcrumbItemTagHelper : MenuItemTagHelperBase
{
    public string Title { get; set; }
    public string Href { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await SetMainTagAsync(context, output, "page-item");
        output.Content.SetHtmlContent(await GetAnchorAsHtmlAsync(Href, "page-link", text: Title));
    }
}