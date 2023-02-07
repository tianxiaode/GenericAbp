using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-nav-menu-header", ParentTag = "metro-nav-menu", TagStructure = TagStructure.WithoutEndTag)]
public class MetroNavMenuHeaderTagHelper : TagHelper
{
    public string Header { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("item-header");
        output.Content.AppendHtml(Header);
        return Task.CompletedTask;
    }
}