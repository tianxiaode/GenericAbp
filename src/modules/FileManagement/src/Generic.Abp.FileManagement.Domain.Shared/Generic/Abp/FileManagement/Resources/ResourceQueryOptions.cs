namespace Generic.Abp.FileManagement.Resources;

public class ResourceQueryOptions(
    bool includeParent = true,
    bool includeFile = false,
    bool includePermissions = false,
    string sorting = "",
    int skipCount = 0,
    int maxResultCount = int.MaxValue)
{
    public bool IncludeParent { get; set; } = includeParent;
    public bool IncludeFile { get; set; } = includeFile;
    public bool IncludePermissions { get; set; } = includePermissions;
    public string Sorting { get; set; } = sorting;
    public int SkipCount { get; set; } = skipCount;
    public int MaxResultCount { get; set; } = maxResultCount;
}