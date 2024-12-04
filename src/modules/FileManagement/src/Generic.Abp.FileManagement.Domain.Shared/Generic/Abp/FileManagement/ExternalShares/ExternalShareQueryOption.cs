namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareQueryOption(
    string? sorting = null,
    int skipCount = 0,
    int maxResultCount = int.MaxValue,
    bool withEntityName = false)
    : QueryOption(sorting, skipCount,
        maxResultCount, withEntityName)
{
    protected override string DefaultSortingPattern => "{0}CreationTime desc";
    protected override string EntityNamePrefix => "FileManagementExternalShares.";
}