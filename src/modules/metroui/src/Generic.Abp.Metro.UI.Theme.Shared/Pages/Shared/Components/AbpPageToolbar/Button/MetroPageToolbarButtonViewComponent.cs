using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers;
using Generic.Abp.Metro.UI.TagHelpers.Button;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar.Button;

public class MetroPageToolbarButtonViewComponent : AbpViewComponent
{
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public MetroPageToolbarButtonViewComponent(IStringLocalizerFactory stringLocalizerFactory)
    {
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        ILocalizableString text,
        string name,
        string icon,
        string id,
        ILocalizableString busyText,
        FontIconType iconType,
        MetroColor color,
        MetroButtonSize size,
        bool disabled)
    {
        Check.NotNull(text, nameof(text));

        return View(
            "~/Pages/Shared/Components/AbpPageToolbar/Button/Default.cshtml",
            new MetroPageToolbarButtonViewModel(
                await text.LocalizeAsync(StringLocalizerFactory) ?? "",
                name,
                icon,
                id,
                busyText == null ? "" : await busyText.LocalizeAsync(StringLocalizerFactory) ?? "",
                iconType,
                color,
                size,
                disabled
            )
        );
    }

    public class MetroPageToolbarButtonViewModel
    {
        public string Text { get; }
        public string Name { get; }
        public string Icon { get; }
        public string Id { get; }
        public string BusyText { get; }
        public FontIconType IconType { get; }
        public MetroColor Color { get; }
        public MetroButtonSize Size { get; }
        public bool Disabled { get; }

        public MetroPageToolbarButtonViewModel(
            string text,
            string name,
            string icon,
            string id,
            string busyText,
            FontIconType iconType,
            MetroColor color,
            MetroButtonSize size,
            bool disabled)
        {
            Text = text;
            Name = name;
            Icon = icon;
            Id = id;
            BusyText = busyText;
            IconType = iconType;
            Color = color;
            Size = size;
            Disabled = disabled;
        }
    }
}