﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("abp-card", Attributes = "background")]
[HtmlTargetElement("abp-card-header", Attributes = "background")]
[HtmlTargetElement("abp-card-body", Attributes = "background")]
[HtmlTargetElement("abp-card-footer", Attributes = "background")]
public class AbpCardBackgroundTagHelper : AbpTagHelper<AbpCardBackgroundTagHelper, AbpCardBackgroundTagHelperService>
{
    public AbpCardBackgroundType Background { get; set; } = AbpCardBackgroundType.Default;

    public AbpCardBackgroundTagHelper(AbpCardBackgroundTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
