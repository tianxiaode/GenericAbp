using System;
using System.ComponentModel;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuCreateDto : MenuCreateOrUpdateDto
{
    [DisplayName("Menu:ParentName")] public Guid? ParentId { get; set; } = default!;
}