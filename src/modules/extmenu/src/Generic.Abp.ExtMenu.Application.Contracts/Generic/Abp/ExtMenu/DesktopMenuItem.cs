using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Generic.Abp.ExtMenu
{
    [Serializable]
    public class DesktopMenuItem :MenuItem
    {
        //        "Id": 500000,
        //        "Text": "MainMenu.OrganizationUnit",
        //        "IconCls": "x-fa fa-car",
        //        "ViewType": "",
        //        "RequiredPermissionName": "MainMenu.OrganizationUnit",
        //        "Selectable": false,
        //        "IsActive": true

        public int? ParentId { get; set; }

        public List<DesktopMenuItem> Children { get; set; }


    }
}
