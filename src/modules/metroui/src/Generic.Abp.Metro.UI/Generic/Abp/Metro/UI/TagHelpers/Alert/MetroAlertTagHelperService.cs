using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Generic.Abp.Metro.UI.TagHelpers.Alert;

public class MetroAlertTagHelperService : MetroTagHelperService<MetroAlertTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        AddClasses(context, output);
        AddDismissButtonIfDismissible(context, output);
    }

    protected override void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        base.AddClasses(context, output);
        output.Attributes.AddClass("info-box");

        var type = TagHelper.AlertType switch
        {
            AlertType.Success => "success" ,
            AlertType.Info => "info",
            AlertType.Warning => "warning",
            AlertType.Danger => "alert",
            _ => "default"
        };
        output.Attributes.Add("data-type", type);

        if (!(TagHelper.Dismissible ?? false)) return;
        output.Attributes.AddClass("fade");
        output.Attributes.AddClass("show");
    }

    protected virtual void AddDismissButtonIfDismissible(TagHelperContext context, TagHelperOutput output)
    {
        if (!TagHelper.Dismissible ?? true)
        {
            return;
        }

        var buttonAsHtml =  $"<span class=\"button square closer\"></span>";

        output.PostContent.SetHtmlContent(buttonAsHtml);
    }
}
