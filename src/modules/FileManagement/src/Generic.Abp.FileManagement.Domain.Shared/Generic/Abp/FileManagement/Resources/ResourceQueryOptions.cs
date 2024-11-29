namespace Generic.Abp.FileManagement.Resources;

public class ResourceQueryOptions(
    bool includeParent = true,
    bool includeFolder = false,
    bool includeFile = false,
    bool includePermissions = false,
    bool includeConfiguration = false)
{
    public bool IncludeParent { get; set; } = includeParent;
    public bool IncludeFolder { get; set; } = includeFolder;
    public bool IncludeFile { get; set; } = includeFile;
    public bool IncludePermissions { get; set; } = includePermissions;
    public bool IncludeConfiguration { get; set; } = includeConfiguration;
}