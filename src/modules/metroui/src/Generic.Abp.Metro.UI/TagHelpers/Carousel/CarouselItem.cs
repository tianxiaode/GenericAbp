namespace Generic.Abp.Metro.UI.TagHelpers.Carousel;

public class CarouselItem
{
    public CarouselItem(string html, bool active)
    {
        Html = html;
        Active = active;
    }

    public string Html { get; set; }

    public bool Active { get; set; }
}
