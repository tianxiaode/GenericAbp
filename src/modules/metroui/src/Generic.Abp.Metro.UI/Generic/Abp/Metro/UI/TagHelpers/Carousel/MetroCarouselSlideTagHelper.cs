using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Carousel;

[HtmlTargetElement("metro-carousel-slide", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroCarouselSlideTagHelper : MetroTagHelper
{
    protected const int DefaultPeriod = 5000;

    public string Cover { get; set; }
    public int Period { get; set; } = DefaultPeriod;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("slide");
        await AddDataAttributeAsync(output, nameof(Cover), Cover);
        if (Period != DefaultPeriod)
        {
            await AddDataAttributeAsync(output, nameof(Period), Period);
        }

        output.Content.AppendHtml(await output.GetChildContentAsync());
    }
}