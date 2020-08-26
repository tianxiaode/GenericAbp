namespace Generic.Abp.ExtMenu
{
    public class MenuItemBaseDto : IMenuItemBaseDto
    {
        public int Id { get; set; }
        public string LangText { get; set; }
        public string IconCls { get; set; }
        public string ViewType { get; set; }

    }
}
