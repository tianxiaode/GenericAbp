using System;

namespace Generic.Abp.ExtResource.Dtos
{
    public class PhoneMenuItemDto : MenuItemDto
    {
        public string Category { get; set; }

        public string Color { get; set; }

        public PhoneMenuItemDto(Guid id, string langText, string iconCls, string viewType, string category, string color) : base(id, langText, iconCls, viewType)
        {
            Category = category;
            Color = color;
        }
    }
}
