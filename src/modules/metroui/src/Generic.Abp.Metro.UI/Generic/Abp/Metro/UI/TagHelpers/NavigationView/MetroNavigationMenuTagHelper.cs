using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavigationMenuTagHelper : MetroNavigationMenuTagHelperBase
{
    public string CurrentValue { get; set; }

    public override void Init(TagHelperContext context)
    {
        context.Items[nameof(MetroNavigationMenuTagHelper)] = CurrentValue;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("navview-menu");
        output.Attributes.AddClass("d-flex");
        output.Attributes.AddClass("flex-column");
        return Task.CompletedTask;
    }
}