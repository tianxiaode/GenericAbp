using System;
using System.Collections.Generic;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuPermissionsUpdateDto
{
    public List<string> Permissions { get; set; } = default!;
}