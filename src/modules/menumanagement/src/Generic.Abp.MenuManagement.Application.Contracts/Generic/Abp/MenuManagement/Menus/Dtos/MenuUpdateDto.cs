using System;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuUpdateDto : MenuCreateOrUpdateDto
{
    public string ConcurrencyStamp { get; set; }
}