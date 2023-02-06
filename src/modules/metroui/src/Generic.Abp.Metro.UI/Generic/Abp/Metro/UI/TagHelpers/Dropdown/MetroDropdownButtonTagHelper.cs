using Generic.Abp.Metro.UI.TagHelpers.Button;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownButtonTagHelper : MetroButtonTagHelper
{
    public MetroDropdownButtonTagHelper(HtmlEncoder htmlEncoder)
    {
        HtmlEncoder = htmlEncoder;
    }

    protected HtmlEncoder HtmlEncoder { get; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.ContainsKey(TagHelperConsts.DropdownIsSplitName))
        {
            var splitButton = await GetSplitButtonAsHtmlAsync(context, output);
            output.PostElement.AppendHtml(splitButton);
            await AddFlagValueAsync(context, nameof(MetroDropdownTagHelper), TagHelperConsts.DropdownIsSplitName);
        }
        else
        {
            output.Attributes.AddClass("dropdown-toggle");
        }

        await base.ProcessAsync(context, output);
    }

    protected virtual async Task<string> GetSplitButtonAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var button = new MetroButtonTagHelper()
        {
            AccentColor = AccentColor,
            IconCls = IconCls,
            Disabled = Disabled,
            Shadowed = Shadowed,
            Outline = Outline,
            Size = Size,
            Rounded = Rounded
        };
        var attributes = new TagHelperAttributeList();
        attributes.AddClass("split");
        attributes.AddClass("dropdown-toggle");

        return await button.RenderAsync(attributes, context,
            HtmlEncoder, "button", TagMode.StartTagAndEndTag);
    }
}