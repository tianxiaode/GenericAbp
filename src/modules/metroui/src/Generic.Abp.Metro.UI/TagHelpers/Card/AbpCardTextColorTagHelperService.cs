﻿using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardTextColorTagHelperService : AbpTagHelperService<AbpCardTextColorTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetTextColor(context, output);
    }

    protected virtual void SetTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.TextColor == AbpCardTextColorType.Default)
        {
            return;
        }

        output.Attributes.AddClass("text-" + TagHelper.TextColor.ToString().ToLowerInvariant());
    }
}
