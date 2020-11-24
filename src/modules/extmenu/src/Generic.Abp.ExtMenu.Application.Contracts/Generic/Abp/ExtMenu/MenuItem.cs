using System;
using System.Collections.Generic;

namespace Generic.Abp.ExtMenu
{
    [Serializable]
    public class MenuItem: IMenuItem
    {
        public int Id { get; set; }
        public string LangText { get; set; }
        public string IconCls { get; set; }
        public string ViewType { get; set; }
        public List<string> RequiredPermissionNames { get; set; }
    }
}