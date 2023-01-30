using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "display-order")]
public class MetroDisplayOrderTagHelper : TagHelper
{
    [HtmlAttributeName("display-order")] public int? DisplayOrder { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (DisplayOrder.HasValue)
        {
            output.Attributes.Add("style", $"order:{DisplayOrder.Value}");
        }

        return Task.CompletedTask;
    }
}