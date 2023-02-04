using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Menu;

[HtmlTargetElement("metro-menu-title", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroMenuTitleTagHelper : MetroMenuTagHelper
{
    public string Title { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("menu-title");
        var title = string.IsNullOrEmpty(Title) ? (await output.GetChildContentAsync()).GetContent() : Title;
        output.Content.SetHtmlContent(title);
    }
}