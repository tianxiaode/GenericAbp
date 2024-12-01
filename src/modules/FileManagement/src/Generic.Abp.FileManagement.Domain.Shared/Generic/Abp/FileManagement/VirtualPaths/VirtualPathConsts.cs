namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathConsts
{
    public static int NameMaxLength { get; set; } = 256;


    private const string DefaultSorting = "{0}Name asc";

    public static string GetDefaultSorting(bool withEntityName = false)
    {
        return string.Format(DefaultSorting, withEntityName ? "FileManagementVirtualPaths." : string.Empty);
    }
}