using System;
using System.Linq;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogTagHelperService : MetroTagHelperService<MetroDialogTagHelper>
{
    public MetroDialogTagHelperService(IStringLocalizer<AbpUiResource> l)
    {
        L = l;
    }

    protected IStringLocalizer<AbpUiResource> L { get; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        output.Attributes.AddClass("dialog");
        //output.Attributes.Add("style", "visibility: hidden; top: 100%;");
        output.Attributes.Add("data-role","dialog");
        output.Attributes.Add("id","{id}");
        return Task.CompletedTask;
    }


}
