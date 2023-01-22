using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonTagHelperService : MetroButtonTagHelperServiceBase<MetroButtonTagHelper>
{
    protected const string DataBusyTextAttributeName = "data-busy-text";
    protected const string DataBusyTextIsHtmlAttributeName = "data-busy-text-is-html";

    protected IStringLocalizer<AbpUiResource> L { get; }

    public MetroButtonTagHelperService(IStringLocalizer<AbpUiResource> localizer)
    {
        L = localizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
        output.TagName = "button";
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
        var busyText = TagHelper.BusyText ?? L["ProcessingWithThreeDot"];
        if (busyText.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        output.Attributes.SetAttribute(DataBusyTextAttributeName, busyText);
        return Task.CompletedTask;
    }

    protected virtual Task AddBusyTextIsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!TagHelper.BusyTextIsHtml)
        {
            return Task.CompletedTask;
        }

        output.Attributes.SetAttribute(DataBusyTextIsHtmlAttributeName, TagHelper.BusyTextIsHtml.ToString().ToLower());
        return Task.CompletedTask;
    }
}
