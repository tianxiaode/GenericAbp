using System;
using Generic.Abp.Domain.Entities.MultiLingual;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuTranslationDto : ITranslation
{
    public string Language { get; set; }

    public string DisplayName { get; set; }

    public MenuTranslationDto()
    {
        Language = "";
    }

    public MenuTranslationDto(string language, string displayName)
    {
        Language = language;
        DisplayName = displayName;
    }
}