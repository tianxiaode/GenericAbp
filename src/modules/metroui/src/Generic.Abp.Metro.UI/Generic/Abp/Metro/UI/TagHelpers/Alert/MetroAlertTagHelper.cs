using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Generic.Abp.Metro.UI.TagHelpers.Alert;

public class MetroAlertTagHelper : TagHelper
{
    public AlertType AlertType { get; set; } = AlertType.Default;

    public bool? Dismissible { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        await AddClassesAsync(context, output);
        await AddDismissButtonIfDismissible(context, output);
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("info-box");

        var type = AlertType switch
        {
            AlertType.Success => "success",
            AlertType.Info => "info",
            AlertType.Warning => "warning",
            AlertType.Danger => "alert",
            _ => "default"
        };
        output.Attributes.Add("data-type", type);

        if (!(Dismissible ?? false)) return Task.CompletedTask;
        output.Attributes.AddClass("fade");
        output.Attributes.AddClass("show");
        return Task.CompletedTask;
    }

    protected virtual Task AddDismissButtonIfDismissible(TagHelperContext context, TagHelperOutput output)
    {
        if (!Dismissible ?? true)
        {
            return Task.CompletedTask;
        }

        const string buttonAsHtml = $"<span class=\"button square closer\"></span>";

        output.PostContent.SetHtmlContent(buttonAsHtml);
        return Task.CompletedTask;
    }
}