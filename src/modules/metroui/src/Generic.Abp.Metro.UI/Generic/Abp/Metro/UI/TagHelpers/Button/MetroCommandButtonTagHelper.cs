using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroCommandButtonTagHelper : ButtonTagHelperBase
{
    public string Caption { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.Attributes.AddClass("command-button");
        await base.ProcessAsync(context, output);
    }
}