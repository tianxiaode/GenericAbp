using Volo.Abp;

namespace Generic.Abp.Metro.UI.TagHelpers;

public static class HtmlHeadingTypeExtensions
{
    public static string ToHtmlTag(this HtmlHeadingType heading)
    {
        return heading switch
        {
            HtmlHeadingType.H1 => "h1",
            HtmlHeadingType.H2 => "h2",
            HtmlHeadingType.H3 => "h3",
            HtmlHeadingType.H4 => "h4",
            HtmlHeadingType.H5 => "h5",
            HtmlHeadingType.H6 => "h6",
            _ => throw new AbpException("Unknown HtmlHeadingType: " + heading)
        };
    }
}
