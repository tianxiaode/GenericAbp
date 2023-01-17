using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonToolbarTagHelperService : MetroButtonGroupTagHelperService
{

    protected override void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        base.AddClasses(context, output);
        output.Attributes.AddClass("toolbar");
    }
}
