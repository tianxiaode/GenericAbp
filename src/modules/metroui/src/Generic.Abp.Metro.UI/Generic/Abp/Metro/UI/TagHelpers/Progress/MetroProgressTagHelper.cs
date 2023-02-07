using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Progress;

[HtmlTargetElement("metro-progress", TagStructure = TagStructure.WithoutEndTag)]
public class MetroProgressTagHelper : MetroTagHelper
{
    protected const string Role = "progress";
    public ProgressType Type { get; set; } = ProgressType.Default;
    public bool ShowValue { get; set; } = false;
    public ProgressValuePosition ValuePosition { get; set; } = ProgressValuePosition.Free;
    public bool ShowLabel { get; set; } = false;
    public ProgressLabelPosition LabelPosition { get; set; } = ProgressLabelPosition.Before;
    public string LabelTemplate { get; set; }
    public int Value { get; set; } = 0;
    public int Buffer { get; set; } = 0;
    public bool Small { get; set; } = false;
    public string ClsBack { get; set; }
    public string ClsBar { get; set; }
    public string ClsBuffer { get; set; }
    public string ClsValue { get; set; }
    public string ClsLabel { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        await ProcessDataAttributesAsync(context, output);
    }

    protected virtual async Task ProcessDataAttributesAsync(TagHelperContext context, TagHelperOutput output)
    {
        await AddDataAttributeAsync(output, nameof(Role), Role);
        if (Type != ProgressType.Default) await AddDataAttributeAsync(output, nameof(Type), Type);

        await AddDataAttributeAsync(output, nameof(ShowValue), ShowValue);
        await AddDataAttributeAsync(output, nameof(ShowLabel), ShowLabel);
        await AddDataAttributeAsync(output, nameof(ValuePosition), ValuePosition);
        await AddDataAttributeAsync(output, nameof(LabelPosition), LabelPosition);
        await AddDataAttributeAsync(output, nameof(LabelTemplate), LabelTemplate, false);
        await AddDataAttributeAsync(output, nameof(Value), Value);
        await AddDataAttributeAsync(output, nameof(Buffer), Buffer);
        await AddDataAttributeAsync(output, nameof(Small), Small);
        await AddDataAttributeAsync(output, nameof(ClsBack), ClsBack);
        await AddDataAttributeAsync(output, nameof(ClsBar), ClsBar);
        await AddDataAttributeAsync(output, nameof(ClsBuffer), ClsBuffer);
        await AddDataAttributeAsync(output, nameof(ClsValue), ClsValue);
        await AddDataAttributeAsync(output, nameof(ClsLabel), ClsLabel);
    }
}