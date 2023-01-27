using System;
using Generic.Abp.Metro.UI.TagHelpers;
using Generic.Abp.Metro.UI.TagHelpers.Button;
using Generic.Abp.Metro.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar.Button;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.Theme.Shared.PageToolbars;

public static class PageToolbarExtensions
{
    public static PageToolbar AddComponent<TComponent>(
        this PageToolbar toolbar,
        object argument = null,
        int order = 0,
        string requiredPolicyName = null)
    {
        return toolbar.AddComponent(
            typeof(TComponent),
            argument,
            order,
            requiredPolicyName
        );
    }

    public static PageToolbar AddComponent(
        this PageToolbar toolbar,
        Type componentType,
        object argument = null,
        int order = 0,
        string requiredPolicyName = null)
    {
        toolbar.Contributors.Add(
            new SimplePageToolbarContributor(
                componentType,
                argument,
                order,
                requiredPolicyName
            )
        );

        return toolbar;
    }

    public static PageToolbar AddButton(
        this PageToolbar toolbar,
        ILocalizableString text,
        string icon = null,
        string name = null,
        string id = null,
        ILocalizableString busyText = null,
        FontIconType iconType = FontIconType.FontAwesome,
        MetroColor color = MetroColor.Primary,
        MetroButtonSize size = MetroButtonSize.Small,
        bool disabled = false,
        int order = 0,
        string requiredPolicyName = null)
    {
        busyText ??= new LocalizableString(typeof(AbpUiResource), "ProcessingWithThreeDot");

        toolbar.AddComponent<MetroPageToolbarButtonViewComponent>(
            new
            {
                text,
                icon,
                name,
                id,
                busyText,
                iconType,
                type = color,
                size,
                disabled
            },
            order,
            requiredPolicyName
        );

        return toolbar;
    }
}