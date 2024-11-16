namespace Generic.Abp.FileManagement.Folders;

public static class FolderConsts
{
    public static int NameMaxLength { get; set; } = 256;
    public static int ProviderTypeMaxLength { get; set; } = 16;
    public static int ProviderNameMaxLength { get; set; } = 64;

    public static int MaxDepth { get; set; } = 32;

    public static int CodeUnitLength { get; set; } = 4;

    public static int CodeMaxLength = 256;

    public static int GetCodeLength(int level)
    {
        return CodeUnitLength * level + level - 1;
    }
}