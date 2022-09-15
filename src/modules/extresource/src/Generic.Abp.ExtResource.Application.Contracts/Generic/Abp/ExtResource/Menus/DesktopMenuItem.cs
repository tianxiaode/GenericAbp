using System;
using System.Collections.Generic;

namespace Generic.Abp.ExtResource.Menus
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

        public Guid? ParentId { get; set; }

        public List<DesktopMenuItem> Children { get; set; }

        public bool Selectable { get; set; }

    }
}
