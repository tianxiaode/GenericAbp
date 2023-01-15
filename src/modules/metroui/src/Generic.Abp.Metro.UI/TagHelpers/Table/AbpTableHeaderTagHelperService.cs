using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Table;

public class AbpTableHeaderTagHelperService : AbpTagHelperService<AbpTableHeaderTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetTheme(context, output);
    }

    protected virtual void SetTheme(TagHelperContext context, TagHelperOutput output)
    {
        switch (TagHelper.Theme)
        {
            case AbpTableHeaderTheme.Default:
                return;
            case AbpTableHeaderTheme.Dark:
                output.Attributes.AddClass("thead-dark");
                return;
            case AbpTableHeaderTheme.Light:
                output.Attributes.AddClass("thead-light");
                return;
        }
    }
}
