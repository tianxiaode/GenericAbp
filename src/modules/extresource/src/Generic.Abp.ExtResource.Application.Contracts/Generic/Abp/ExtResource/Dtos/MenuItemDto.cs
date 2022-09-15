using System;

namespace Generic.Abp.ExtResource.Dtos
{
    public class MenuItemDto : IMenuItemDto
    {
        public Guid Id { get; set; }
        public string LangText { get; set; }
        public string IconCls { get; set; }
        public string ViewType { get; set; }


        public MenuItemDto()
        {
        }

        public MenuItemDto(Guid id, string langText, string iconCls, string viewType)
        {
            Id = id;
            LangText = langText;
            IconCls = iconCls;
            ViewType = viewType;
        }
    }
}
