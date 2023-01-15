using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Table;

public class AbpTableStyleTagHelperService : AbpTagHelperService<AbpTableStyleTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetStyle(context, output);
    }

    protected virtual void SetStyle(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.TableStyle != AbpTableStyle.Default)
        {
            output.Attributes.AddClass("table-" + TagHelper.TableStyle.ToString().ToLowerInvariant());
        }
    }
}
