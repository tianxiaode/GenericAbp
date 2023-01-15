using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Border;

public class AbpRoundedTagHelperService : AbpTagHelperService<AbpRoundedTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var roundedClass = "rounded";

        if (TagHelper.AbpRounded != AbpRoundedType.Default)
        {
            roundedClass += "-" + TagHelper.AbpRounded.ToString().ToLowerInvariant().Replace("_", "");
        }

        output.Attributes.AddClass(roundedClass);
    }
}
