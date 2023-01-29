using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Panel;

public class MetroPanelTagHelper : MetroTagHelper
{
    public string TitleCls { get; set; }
    public string TitleCaption { get; set; }
    public string TitleCaptionCls { get; set; }
    public string TitleIcon { get; set; }
    public string TitleIconCls { get; set; }
    public bool? Collapsed { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public string ContentCls { get; set; }
    public bool? Draggable { get; set; }
    public string OnDragMove { get; set; }
    public string OnCollapse { get; set; }
    public string OnExpand { get; set; }
    public string OnDragStart { get; set; }
    public string OnDragStop { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        await AddAttributesAsync(context, output);
    }

    protected virtual Task AddAttributesAsync(TagHelperContext context, TagHelperOutput output)
    {
        var attributes = output.Attributes;
        attributes.Add("data-role", "panel");
        if (!string.IsNullOrWhiteSpace(TitleCaption))
        {
            attributes.Add("data-title-caption", TitleCaption);
        }

        if (!string.IsNullOrWhiteSpace(TitleCls))
        {
            attributes.Add("data-cls-title", TitleCls);
        }

        if (!string.IsNullOrWhiteSpace(TitleCaptionCls))
        {
            attributes.Add("data-cls-title-caption", TitleCaptionCls);
        }

        if (!string.IsNullOrWhiteSpace(TitleIcon))
        {
            attributes.Add("data-title-icon", TitleIcon);
        }

        if (!string.IsNullOrWhiteSpace(TitleIconCls))
        {
            attributes.Add("data-cls-title-icon", TitleIconCls);
        }

        if (Collapsed == true)
        {
            attributes.Add("data-collapsible", true);
            if (!string.IsNullOrWhiteSpace(OnCollapse)) attributes.Add("data-on-collapse", OnCollapse);
            if (!string.IsNullOrWhiteSpace(OnExpand)) attributes.Add("data-on-expand", OnExpand);
        }

        if (Height.HasValue)
        {
            attributes.Add("data-height", Height.Value);
        }

        if (Width.HasValue)
        {
            attributes.Add("data-width", Width.Value);
        }

        if (!string.IsNullOrWhiteSpace(ContentCls))
        {
            attributes.Add("data-cls-content", ContentCls);
        }

        if (Draggable != true) return Task.CompletedTask;
        attributes.Add("data-draggable", true);
        if (!string.IsNullOrWhiteSpace(OnDragMove)) attributes.Add("data-on-drag-move", OnDragMove);
        if (!string.IsNullOrWhiteSpace(OnDragStart)) attributes.Add("data-on-drag-start", OnDragStart);
        if (!string.IsNullOrWhiteSpace(OnDragStop)) attributes.Add("data-on-drag-stop", OnDragStop);

        return Task.CompletedTask;
    }
}