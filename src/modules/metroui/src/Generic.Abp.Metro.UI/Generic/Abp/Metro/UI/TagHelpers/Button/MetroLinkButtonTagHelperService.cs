﻿using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroLinkButtonTagHelperService : MetroButtonTagHelperServiceBase<MetroLinkButtonTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
        AddType(context, output);
        AddRole(context, output);
    }

    protected virtual void AddType(TagHelperContext context, TagHelperOutput output)
    {
        if (!output.Attributes.ContainsName("type") &&
            output.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase))
        {
            output.Attributes.Add("type", "button");
        }
    }

    protected virtual void AddRole(TagHelperContext context, TagHelperOutput output)
    {
        if (!output.Attributes.ContainsName("role"))
        {
            output.Attributes.Add("role", "button");
        }
    }
}
