using System;

namespace Generic.Abp.ExtResource.Menus
{
    public interface IMenuItemBase
    {
        Guid Id { get; set; }
        string LangText { get; set; }
        string IconCls { get; set; }
        string ViewType { get; set; }
    }
}