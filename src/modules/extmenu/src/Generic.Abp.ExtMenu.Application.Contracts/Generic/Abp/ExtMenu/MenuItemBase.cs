using System;
using System.Collections.Generic;

namespace Generic.Abp.ExtMenu
{
    [Serializable]
    public class MenuItemBase : MenuItemBaseDto, IMenuItemBase
    {

        public List<string> RequiredPermissionNames { get; set; }

        public MenuItemBase()
        {
            RequiredPermissionNames = new List<string>();
        }
    }
}
