namespace Generic.Abp.FileManagement.Resources;

public class ResourceQueryOption(
    bool includeParent = true,
    bool includeFile = false,
    bool includePermissions = false,
    string sorting = "",
    int skipCount = 0,
    int maxResultCount = int.MaxValue) : QueryOption(sorting, skipCount, maxResultCount)
{
    public bool IncludeParent { get; set; } = includeParent;
    public bool IncludeFile { get; set; } = includeFile;
    public bool IncludePermissions { get; set; } = includePermissions;
}