using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Popover;

[HtmlTargetElement(Attributes = "popover")]
public class PopoverTagHelper : MetroTagHelper
{
    protected const int DefaultPopoverHide = 3000;
    protected const int DefaultPopoverTimeout = 10;
    protected const int DefaultPopoverOffset = 10;
    protected const string Role = "popover";
    [HtmlAttributeName("popover")] public string Popover { get; set; }
    public int PopoverHide { get; set; } = DefaultPopoverHide;
    public int PopoverTimeout { get; set; } = DefaultPopoverTimeout;
    public int PopoverOffset { get; set; } = DefaultPopoverOffset;
    public PopoverTrigger PopoverTrigger { get; set; } = PopoverTrigger.Hover;
    public PopoverPosition PopoverPosition { get; set; } = PopoverPosition.Top;
    public bool PopoverHideOnLeave { get; set; } = false;
    public bool PopoverCloseButton { get; set; } = true;
    public string PopoverCls { get; set; }
    public string PopoverContentCls { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Popover)) return;
        await AddDataAttributeAsync(output, nameof(Role), Role);
        await AddDataAttributeAsync(output, "PopoverText", Popover);
        if (PopoverHide != DefaultPopoverHide) await AddDataAttributeAsync(output, nameof(PopoverHide), PopoverHide);
        if (PopoverTimeout != DefaultPopoverTimeout)
            await AddDataAttributeAsync(output, nameof(PopoverTimeout), PopoverTimeout);
        if (PopoverOffset != DefaultPopoverOffset)
            await AddDataAttributeAsync(output, nameof(PopoverOffset), PopoverOffset);
        if (PopoverTrigger != PopoverTrigger.Hover)
            await AddDataAttributeAsync(output, nameof(PopoverTrigger), PopoverTrigger);
        if (PopoverPosition != PopoverPosition.Top)
            await AddDataAttributeAsync(output, nameof(PopoverPosition), PopoverPosition);
        await AddDataAttributeAsync(output, "HideOnLeave", PopoverHideOnLeave);
        await AddDataAttributeAsync(output, "CloseButton", PopoverCloseButton);
        await AddDataAttributeAsync(output, "ClsPopover", PopoverCls);
        await AddDataAttributeAsync(output, "ClsPopoverContent", PopoverContentCls);
    }
}