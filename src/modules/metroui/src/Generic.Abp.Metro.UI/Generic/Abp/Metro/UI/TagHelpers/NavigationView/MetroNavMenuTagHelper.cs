using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavMenuTagHelper : TagHelper
{
    public string Current { get; set; }

    public override void Init(TagHelperContext context)
    {
        context.Items[nameof(MetroNavMenuTagHelper)] = Current;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("navview-menu");
        output.Attributes.AddClass("d-flex");
        output.Attributes.AddClass("flex-column");
        return Task.CompletedTask;
    }
}