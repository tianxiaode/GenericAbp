﻿using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Blockquote;

public class AbpBlockquoteTagHelperService : AbpTagHelperService<AbpBlockquoteTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("blockquote");
        output.TagName = "blockquote";
    }
}
