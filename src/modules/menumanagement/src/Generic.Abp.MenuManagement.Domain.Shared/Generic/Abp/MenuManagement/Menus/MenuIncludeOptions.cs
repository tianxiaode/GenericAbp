using Generic.Abp.Extensions.Entities.IncludeOptions;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuIncludeOptions(bool includeParent = true, bool includeChildren = false) : IIncludeOptions
{
    public bool IncludeParent { get; set; } = includeParent;
    public bool IncludeChildren { get; set; } = includeChildren;
}