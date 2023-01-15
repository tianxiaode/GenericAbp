using Volo.Abp;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public static class AbpCardImagePositionExtensions
{
    public static string ToClassName(this AbpCardImagePosition position)
    {
        switch (position)
        {
            case AbpCardImagePosition.None:
                return "card-img";
            case AbpCardImagePosition.Top:
                return "card-img-top";
            case AbpCardImagePosition.Bottom:
                return "card-img-bottom";
            default:
                throw new AbpException("Unknown AbpCardImagePosition: " + position);
        }
    }
}
