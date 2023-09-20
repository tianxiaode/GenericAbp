using System;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

public class MenuGetListInput
{
    public Guid? Node { get; set; }
    public string Filter { get; set; }
    public string GroupName { get; set; }
}