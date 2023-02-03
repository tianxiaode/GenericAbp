using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Button;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Collapse;

public class MetroCollapseTagHelper : ButtonTagHelperBase
{
    protected const string Role = "collapse";
    protected const string ToggleElement = "#";
    protected string Id { get; set; }
    public string BodyCls { get; set; }
    public bool Collapsed { get; set; } = true;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Id = $"collapse-{Guid.NewGuid()}";
        output.TagName = "button";
        output.Attributes.Add("id", Id);
        await base.ProcessAsync(context, output);
        var child = await output.GetChildContentAsync();
        output.PostElement.SetHtmlContent(await GetContentAsync(context, output, child.GetContent()));
    }

    protected virtual async Task<string> GetContentAsync(TagHelperContext context, TagHelperOutput output,
        string content)
    {
        var stringBuilder = new StringBuilder();
        //var buttonHtml = await AddButtonAsync(context, output);
        //stringBuilder.AppendLine(buttonHtml);
        stringBuilder.AppendLine($" <div class=\"pos-relative\">");
        stringBuilder.AppendLine(await GetBodyAsync(context, output, content));
        stringBuilder.AppendLine($" </div>");

        return stringBuilder.ToString();
    }

    protected virtual async Task<string> GetBodyAsync(TagHelperContext context, TagHelperOutput output,
        string content)
    {
        var body = new TagBuilder("div");
        if (!string.IsNullOrWhiteSpace(BodyCls))
        {
            body.AddCssClass(BodyCls);
        }

        if (Collapsed) await AddDataAttributeAsync(body, nameof(Collapsed), Collapsed);

        await AddDataAttributeAsync(body, nameof(Role), Role);
        await AddDataAttributeAsync(body, nameof(ToggleElement), $"{ToggleElement}{Id}");
        body.InnerHtml.AppendHtml(content);

        return body.ToHtmlString();
    }
}