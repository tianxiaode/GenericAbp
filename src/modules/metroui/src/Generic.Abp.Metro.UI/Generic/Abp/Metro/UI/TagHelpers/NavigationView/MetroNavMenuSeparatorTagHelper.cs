using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-nav-menu-separator", ParentTag = "metro-nav-menu", TagStructure = TagStructure.WithoutEndTag)]
public class MetroNavMenuSeparatorTagHelper : TagHelper
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("item-separator");
        return Task.CompletedTask;
    }
}