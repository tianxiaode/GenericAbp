using System.Collections.Generic;

namespace Generic.Abp.ExtResource.Menus
{
    public interface IMenuItem: IMenuItemBase
    {
        List<string> RequiredPermissionNames { get; set; }

    }
}