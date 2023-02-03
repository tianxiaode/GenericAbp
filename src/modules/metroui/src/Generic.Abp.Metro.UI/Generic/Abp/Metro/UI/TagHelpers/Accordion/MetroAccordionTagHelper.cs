using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Accordion;

public class MetroAccordionTagHelper : MetroTagHelper
{
    protected string Role = "accordion";
    public bool ShowMarker { get; set; } = true;
    public bool Material { get; set; } = false;
    public bool OneFrame { get; set; } = true;
    public bool ShowActive { get; set; } = true;
    public string ActiveFrameClass { get; set; }
    public string ActiveHeadingClass { get; set; }
    public string ActiveContentClass { get; set; }
    public string ClsFrame { get; set; }
    public string ClsHeading { get; set; }
    public string ClsContent { get; set; }
    public string ClsAccordion { get; set; }
    public string ClsActiveFrame { get; set; }
    public string ClsActiveFrameHeading { get; set; }
    public string ClsActiveFrameContent { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddDataAttributeAsync(output, nameof(Role), Role);
        var child = await output.GetChildContentAsync();
        if (!ShowMarker) await AddDataAttributeAsync(output, nameof(ShowMarker), ShowMarker);
        if (Material) await AddDataAttributeAsync(output, nameof(Material), Material);
        if (!OneFrame) await AddDataAttributeAsync(output, nameof(OneFrame), OneFrame);
        if (!ShowActive) await AddDataAttributeAsync(output, nameof(ShowActive), ShowActive);
        await AddDataAttributeAsync(output, nameof(ActiveFrameClass), ActiveFrameClass);
        await AddDataAttributeAsync(output, nameof(ActiveHeadingClass), ActiveHeadingClass);
        await AddDataAttributeAsync(output, nameof(ActiveContentClass), ActiveContentClass);
        await AddDataAttributeAsync(output, nameof(ClsFrame), ClsFrame);
        await AddDataAttributeAsync(output, nameof(ClsHeading), ClsHeading);
        await AddDataAttributeAsync(output, nameof(ClsContent), ClsContent);
        await AddDataAttributeAsync(output, nameof(ClsAccordion), ClsAccordion);
        await AddDataAttributeAsync(output, nameof(ClsActiveFrame), ClsActiveFrame);
        await AddDataAttributeAsync(output, nameof(ClsActiveFrameHeading), ClsActiveFrameHeading);
        await AddDataAttributeAsync(output, nameof(ClsActiveFrameContent), ClsActiveFrameContent);
        output.Content.AppendHtml(child);
    }
}