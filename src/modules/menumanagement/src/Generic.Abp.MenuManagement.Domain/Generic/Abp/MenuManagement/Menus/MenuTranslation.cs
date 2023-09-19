using System;
using System.Diagnostics.CodeAnalysis;
using Generic.Abp.Domain.Entities;

namespace Generic.Abp.MenuManagement.Menus;

[Serializable]
public class MenuTranslation : Translation
{
    public string DisplayName { get; protected set; }

    protected MenuTranslation(string language) : base(language)
    {
    }

    public MenuTranslation([NotNull] string language, string displayName) : base(language)
    {
        DisplayName = displayName;
    }
}