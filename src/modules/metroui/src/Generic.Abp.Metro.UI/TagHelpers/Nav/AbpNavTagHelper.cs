﻿namespace Generic.Abp.Metro.UI.TagHelpers.Nav;

public class AbpNavTagHelper : AbpTagHelper<AbpNavTagHelper, AbpNavTagHelperService>
{
    public AbpNavAlign Align { get; set; } = AbpNavAlign.Default;

    public NavStyle NavStyle { get; set; } = NavStyle.Default;

    public AbpNavTagHelper(AbpNavTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
