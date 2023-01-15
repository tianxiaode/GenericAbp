using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardBackgroundTagHelperService : AbpTagHelperService<AbpCardBackgroundTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetBackground(context, output);
    }

    protected virtual void SetBackground(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Background == AbpCardBackgroundType.Default)
        {
            return;
        }

        output.Attributes.AddClass("bg-" + TagHelper.Background.ToString().ToLowerInvariant());
    }
}
