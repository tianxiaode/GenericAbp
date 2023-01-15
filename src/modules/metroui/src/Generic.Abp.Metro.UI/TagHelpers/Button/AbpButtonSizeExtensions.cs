using Volo.Abp;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public static class AbpButtonSizeExtensions
{
    public static string ToClassName(this AbpButtonSize size)
    {
        switch (size)
        {
            case AbpButtonSize.Mini:
                return "mini";
            case AbpButtonSize.Small:
                return "small";
            case AbpButtonSize.Large:
                return "large";
            case AbpButtonSize.Default:
                return "";
            case AbpButtonSize.SizeSmall:
                return "size-small";
            case AbpButtonSize.SizeLarge:
                return "size-large";
            default:
                throw new AbpException($"Unknown {nameof(AbpButtonSize)}: {size}");
        }
    }
}
