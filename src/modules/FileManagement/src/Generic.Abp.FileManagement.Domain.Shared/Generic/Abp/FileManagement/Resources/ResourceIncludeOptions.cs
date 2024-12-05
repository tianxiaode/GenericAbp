using Generic.Abp.Extensions.Entities.IncludeOptions;

namespace Generic.Abp.FileManagement.Resources;

public class ResourceIncludeOptions(
    bool includeParent = true,
    bool includeFile = false,
    bool includePermissions = false) : IIncludeOptions
{
    public bool IncludeParent { get; set; } = includeParent;
    public bool IncludeFile { get; set; } = includeFile;
    public bool IncludePermissions { get; set; } = includePermissions;

    public static ResourceIncludeOptions Default => new(false);
}