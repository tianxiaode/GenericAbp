namespace Generic.Abp.MenuManagement.Menus;

public static class MenuConsts
{
    public const string Permissions = "Permissions";
    public static int IconMaxLength { get; set; } = 128;
    public static int RouterMaxLength { get; set; } = 128;

    private const string DefaultSorting = "{0}Order asc";

    public static string GetDefaultSorting(bool withEntityName = false)
    {
        return string.Format(DefaultSorting, withEntityName ? $"MenuManagementMenus." : string.Empty);
    }
}