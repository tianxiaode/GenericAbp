namespace Generic.Abp.ExtResource.Dtos
{
    public class MenuItemDto : IMenuItemDto
    {
        public int Id { get; set; }
        public string LangText { get; set; }
        public string IconCls { get; set; }
        public string ViewType { get; set; }

    }
}
