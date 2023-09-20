using System;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuTranslationUpdateDto
{
    public string Language { get; set; }
    public string DisplayName { get; set; }
}