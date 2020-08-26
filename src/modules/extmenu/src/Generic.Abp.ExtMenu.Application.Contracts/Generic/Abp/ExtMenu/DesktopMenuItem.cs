using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Generic.Abp.ExtMenu
{
    [Serializable]
    public class DesktopMenuItem : MenuItemBase
    {
        //        "Id": 500000,
        //        "Text": "MainMenu.OrganizationUnit",
        //        "IconCls": "x-fa fa-car",
        //        "ViewType": "",
        //        "RequiredPermissionName": "MainMenu.OrganizationUnit",
        //        "Selectable": false,
        //        "IsActive": true

        public int? ParentId { get; set; }

        public IEnumerable<DesktopMenuItem> Children { get; set; }

        [DefaultValue(true)]
        public bool Selectable { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(true)]
        public bool Leaf { get; set; }

    }
}
