using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Carousel;

public class MetroCarouselTagHelper : MetroTagHelper
{
    protected const int DefaultDuration = 100;
    protected const int DefaultPeriod = 5000;
    protected const string DefaultWidth = "100%";
    protected const string DefaultHeight = "16/9";
    protected const string Role = "carousel";
    public bool AutoStart { get; set; } = true;
    public string Width { get; set; } = DefaultWidth;
    public string Height { get; set; } = DefaultHeight;
    public CarouselEffect Effect { get; set; } = CarouselEffect.Slide;
    public CarouselDirection Direction { get; set; } = CarouselDirection.Left;
    public int Duration { get; set; } = DefaultDuration;
    public int Period { get; set; } = DefaultPeriod;
    public bool StopOnMouse { get; set; } = true;
    public bool Controls { get; set; } = true;
    public bool Bullets { get; set; } = true;
    public CarouselBulletsStyle BulletsStyle { get; set; } = CarouselBulletsStyle.Square;
    public CarouselBulletsSize BulletsSize { get; set; } = CarouselBulletsSize.Default;
    public CarouselBulletsPosition BulletsPosition { get; set; } = CarouselBulletsPosition.Default;
    public bool ControlsOnMouse { get; set; } = false;
    public bool ControlsOutside { get; set; } = false;
    public string ControlPrev { get; set; }
    public string ControlNext { get; set; }
    public string ClsControls { get; set; }
    public string ClsControlNext { get; set; }
    public string ClsControlPrev { get; set; }
    public string ClsBullets { get; set; }
    public string ClsBullet { get; set; }
    public string ClsBulletOn { get; set; }
    public string ClsThumbOn { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        await ProcessBaseAttributesAsync(context, output);
        await ProcessControlsAttributeAsync(context, output);
        await ProcessBulletsAttributeAsync(context, output);
    }

    protected virtual async Task ProcessBaseAttributesAsync(TagHelperContext context, TagHelperOutput output)
    {
        await AddDataAttributeAsync(output, nameof(Role), Role);
        if (AutoStart)
            await AddDataAttributeAsync(output, nameof(AutoStart), true);
        if (Width != DefaultWidth)
        {
            await AddDataAttributeAsync(output, nameof(Width), Width);
        }

        if (Height != DefaultHeight)
        {
            await AddDataAttributeAsync(output, nameof(Height), Height);
        }

        if (Effect != CarouselEffect.Slide)
        {
            await AddDataAttributeAsync(output, nameof(Effect), Effect == CarouselEffect.SlideV ? "slide-v" : Effect);
        }

        if (Direction != CarouselDirection.Left)
        {
            await AddDataAttributeAsync(output, nameof(Direction), Direction);
        }

        if (Duration != DefaultDuration)
        {
            await AddDataAttributeAsync(output, nameof(Duration), Duration);
        }

        if (Period != DefaultPeriod)
        {
            await AddDataAttributeAsync(output, nameof(Period), Period);
        }

        if (!StopOnMouse)
            await AddDataAttributeAsync(output, nameof(StopOnMouse), StopOnMouse);

        await AddDataAttributeAsync(output, nameof(ClsThumbOn), ClsThumbOn);
    }

    protected virtual async Task ProcessControlsAttributeAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!Controls)
            await AddDataAttributeAsync(output, nameof(Controls), Controls);

        if (ControlsOnMouse)
        {
            await AddDataAttributeAsync(output, nameof(ControlsOnMouse), ControlsOnMouse);
        }

        if (ControlsOutside)
        {
            await AddDataAttributeAsync(output, nameof(ControlsOutside), ControlsOutside);
        }

        await AddDataAttributeAsync(output, nameof(ControlPrev), ControlPrev);
        await AddDataAttributeAsync(output, nameof(ControlNext), ControlNext);
        await AddDataAttributeAsync(output, nameof(ClsControls), ClsControls);
        await AddDataAttributeAsync(output, nameof(ClsControlNext), ClsControlNext);
        await AddDataAttributeAsync(output, nameof(ClsControlPrev), ClsControlPrev);
    }

    protected virtual async Task ProcessBulletsAttributeAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!Bullets)
            await AddDataAttributeAsync(output, nameof(Bullets), Bullets);

        if (BulletsStyle != CarouselBulletsStyle.Square)
        {
            await AddDataAttributeAsync(output, nameof(BulletsStyle), BulletsStyle);
        }

        if (BulletsSize != CarouselBulletsSize.Default)
        {
            await AddDataAttributeAsync(output, nameof(BulletsSize), BulletsSize);
        }

        if (BulletsPosition != CarouselBulletsPosition.Default)
        {
            await AddDataAttributeAsync(output, nameof(BulletsPosition), BulletsPosition);
        }

        await AddDataAttributeAsync(output, nameof(ClsBullets), ClsBullets);
        await AddDataAttributeAsync(output, nameof(ClsBullet), ClsBullet);
        await AddDataAttributeAsync(output, nameof(ClsBulletOn), ClsBulletOn);
    }
}