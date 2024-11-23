namespace Generic.Abp.FileManagement.Resources;

public static class ResourceConsts
{
    public const string PublicRootFolderName = "PUBLIC";
    public const string UsersRootFolderName = "USERS";
    public const string SharedRootFolderName = "SHARED";
    public const string VirtualRootFolderName = "VIRTUAL";

    public const string UserProviderName = "U";
    public const string RoleProviderName = "R";
    public const string AuthorizationUserProviderName = "A";
    public const string EveryoneProviderName = "E";

    public static int NameMaxLength { get; set; } = 256;
    public static int ProviderNameMaxLength { get; set; } = 16;
    public static int ProviderKeyMaxLength { get; set; } = 64;
    public static int AllowedFileTypesMaxLength { get; set; } = 2048;
    public static int MaxDepth { get; set; } = 32;

    public static int CodeUnitLength { get; set; } = 6;

    public static int CodeMaxLength => GetCodeLength(32);

    public static int GetCodeLength(int level)
    {
        return CodeUnitLength * level + level - 1;
    }
}