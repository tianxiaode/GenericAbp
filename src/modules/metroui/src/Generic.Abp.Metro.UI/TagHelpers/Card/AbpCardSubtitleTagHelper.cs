namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class AbpCardSubtitleTagHelper : AbpTagHelper<AbpCardSubtitleTagHelper, AbpCardSubtitleTagHelperService>
{
    public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H6;

    public HtmlHeadingType Heading { get; set; } = DefaultHeading;

    public AbpCardSubtitleTagHelper(AbpCardSubtitleTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
