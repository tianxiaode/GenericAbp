namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareConsts
{
    public static int SharedTokenMaxLength { get; set; } = 36;
    public static int PasswordMaxLength { get; set; } = 4;

    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName = false)
    {
        return string.Format(DefaultSorting, withEntityName ? "FileManagementExternalShares." : string.Empty);
    }
}