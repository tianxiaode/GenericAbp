using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

[HtmlTargetElement("metro-button", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroButtonTagHelper : ButtonTagHelperBase
{
    public MetroButtonTagHelper(IStringLocalizer<AbpUiResource> l)
    {
        L = l;
    }

    protected IStringLocalizer<AbpUiResource> L { get; }
    public string BusyText { get; set; }
    public bool BusyTextIsHtml { get; set; }

    protected const string DataBusyTextAttributeName = "data-busy-text";
    protected const string DataBusyTextIsHtmlAttributeName = "data-busy-text-is-html";


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        await base.ProcessAsync(context, output);
        await AddTypeAsync(context, output);
        await AddBusyTextAsync(context, output);
        await AddBusyTextIsHtmlAsync(context, output);
    }

    protected virtual Task AddTypeAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (output.Attributes.ContainsName("type"))
        {
            return Task.CompletedTask;
        }

        output.Attributes.Add("type", "button");

        return Task.CompletedTask;
    }

    protected virtual Task AddBusyTextAsync(TagHelperContext context, TagHelperOutput output)
    {
        var busyText = BusyText ?? L["ProcessingWithThreeDot"];
        if (busyText.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        output.Attributes.SetAttribute(DataBusyTextAttributeName, busyText);
        return Task.CompletedTask;
    }

    protected virtual Task AddBusyTextIsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!BusyTextIsHtml)
        {
            return Task.CompletedTask;
        }

        output.Attributes.SetAttribute(DataBusyTextIsHtmlAttributeName, BusyTextIsHtml.ToString().ToLower());
        return Task.CompletedTask;
    }
}