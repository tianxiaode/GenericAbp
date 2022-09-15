using System;
using System.Collections.Generic;

namespace Generic.Abp.ExtResource.Menus
{
    [Serializable]
    public class MenuItem: IMenuItem
    {
        public Guid Id { get; set; }
        public string LangText { get; set; }
        public string IconCls { get; set; }
        public string ViewType { get; set; }
        public int Order { get; set; }
        public List<string> RequiredPermissionNames { get; set; }

        public MenuItem()
        {
            RequiredPermissionNames = new List<string>();
        }
    }
}