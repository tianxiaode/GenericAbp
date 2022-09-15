using System.Collections.Generic;
using Generic.Abp.ExtResource.Menus;

namespace Generic.Abp.ExtResource
{
    public class MenuOptions
    {
        public List<DesktopMenuItem> Desktop { get; set; }

        public List<PhoneMenuItem> Mobile { get; set; }

    }
}
