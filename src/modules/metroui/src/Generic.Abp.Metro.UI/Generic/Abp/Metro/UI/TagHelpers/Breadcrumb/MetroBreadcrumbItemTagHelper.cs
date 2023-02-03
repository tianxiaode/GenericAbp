using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

[HtmlTargetElement("metro-breadcrumb-item", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroBreadcrumbItemTagHelper : MetroTagHelper<BreadcrumbGroupItem>
{
    public MetroBreadcrumbItemTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
    }

    public string Title { get; set; }
    public string Href { get; set; }

    public override void Init(TagHelperContext context)
    {
        GroupItemsName = TagHelperConsts.BreadcrumbItems;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "li";
        output.Attributes.AddClass("page-item");
        output.Content.SetHtmlContent(await GetInnerHtmlAsync(context, output));
        await AddGroupItemAsync(context, Title, await output.RenderAsync(HtmlEncoder));
        output.SuppressOutput();
    }

    protected virtual Task<string> GetInnerHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Href))
        {
            return Task.FromResult(HtmlEncoder.Encode(Title));
        }

        var link = new TagBuilder("a");
        link.Attributes.Add("href", Href);
        link.InnerHtml.AppendHtml(Title);
        return Task.FromResult(link.ToHtmlString());
    }
}