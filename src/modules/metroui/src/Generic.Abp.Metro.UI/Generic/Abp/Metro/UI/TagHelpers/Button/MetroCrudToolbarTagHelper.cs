using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroCrudToolbarTagHelper : MetroTagHelper
{
    public MetroCrudToolbarTagHelper(HtmlEncoder htmlEncoder, IStringLocalizer<AbpUiResource> l)
    {
        HtmlEncoder = htmlEncoder;
        L = l;
    }

    protected HtmlEncoder HtmlEncoder { get; }
    protected IStringLocalizer<AbpUiResource> L { get; }

    public bool Refresh { get; set; } = true;
    public string RefreshText { get; set; } = "Refresh";
    public string RefreshIcon { get; set; } = "fa fa-undo";
    public MetroColor RefreshColor { get; set; } = MetroColor.Primary;
    public bool New { get; set; } = true;
    public string NewText { get; set; } = "New";
    public string NewIcon { get; set; } = "fa fa-file primary";
    public MetroColor NewColor { get; set; } = MetroColor.Primary;
    public bool Edit { get; set; } = false;
    public string EditText { get; set; } = "Edit";
    public string EditIcon { get; set; } = "fa fa-edit";
    public MetroColor EditColor { get; set; } = MetroColor.Primary;
    public bool Delete { get; set; } = true;
    public string DeleteText { get; set; } = "Delete";
    public string DeleteIcon { get; set; } = "fa fa-trash";
    public MetroColor DeleteColor { get; set; } = MetroColor.Alert;
    public bool ShowText { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("crud-toolbar");
        output.Attributes.Add("data-role", "crud-toolbar");
        output.Attributes.Add("data-on-button-click", "onButtonClick");
        var html = await GetButtonsAsHtmlAsync(context, output);
        var child = await output.GetChildContentAsync();
        output.Content.AppendHtml(html);
        output.Content.AppendHtml(child);
    }

    public virtual async Task<string> GetButtonsAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var stringBuilder = new StringBuilder();
        if (Refresh)
        {
            stringBuilder.AppendLine(await AddButtonAsync(context, output, RefreshText, RefreshIcon,
                RefreshColor, await GetDisplayOrderAsync(0)));
        }

        if (New)
        {
            stringBuilder.AppendLine(await AddButtonAsync(context, output, NewText, NewIcon,
                NewColor, await GetDisplayOrderAsync(0)));
        }

        if (Edit)
        {
            stringBuilder.AppendLine(await AddButtonAsync(context, output, EditText, EditIcon,
                EditColor, await GetDisplayOrderAsync(0)));
        }

        if (Delete)
        {
            stringBuilder.AppendLine(await AddButtonAsync(context, output, DeleteText, DeleteIcon,
                DeleteColor, await GetDisplayOrderAsync(0)));
        }

        return stringBuilder.ToString();
    }

    public virtual async Task<string> AddButtonAsync(TagHelperContext context, TagHelperOutput output, string text,
        string icon, MetroColor color, int displayOrder)
    {
        var button = new MetroButtonTagHelper()
        {
            IconType = FontIconType.Other,
            Color = color,
            Icon = icon,
            Text = ShowText ? L[text] : "",
        };
        var buttonOutput = await button.ProcessAndGetOutputAsync(new TagHelperAttributeList(), context, "button",
            TagMode.StartTagAndEndTag);
        buttonOutput.Attributes.Add("style", $"order:{displayOrder}");
        buttonOutput.Attributes.Add("data-action", text.ToLowerInvariant());
        buttonOutput.Attributes.AddClass("mr-1");
        return await buttonOutput.RenderAsync(HtmlEncoder);
    }
}