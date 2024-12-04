using Generic.Abp.Extensions.Entities.IncludeOptions;

namespace Generic.Abp.Extensions.Entities.Trees;

public class TreeIncludeOptions(bool includeParent = true, bool includeChildren = false) : IIncludeOptions
{
    public bool IncludeChildren { get; set; } = includeChildren;
    public bool IncludeParent { get; set; } = includeParent;
}