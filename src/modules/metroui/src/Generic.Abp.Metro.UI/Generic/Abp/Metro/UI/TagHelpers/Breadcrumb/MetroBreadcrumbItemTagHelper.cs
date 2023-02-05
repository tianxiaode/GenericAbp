using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

[HtmlTargetElement("metro-breadcrumb-item", TagStructure = TagStructure.WithoutEndTag)]
public class MetroBreadcrumbItemTagHelper : MetroTagHelper
{
    public string Title { get; set; }
    public string Href { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "li";
        output.Attributes.AddClass("page-item");
        output.Content.SetHtmlContent(await GetInnerHtmlAsync(context, output));
    }

    protected virtual Task<string> GetInnerHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var link = new TagBuilder("a");
        if (!string.IsNullOrWhiteSpace(Href)) link.Attributes.Add("href", Href);
        link.InnerHtml.AppendHtml(Title);
        return Task.FromResult(link.ToHtmlString());
    }
}