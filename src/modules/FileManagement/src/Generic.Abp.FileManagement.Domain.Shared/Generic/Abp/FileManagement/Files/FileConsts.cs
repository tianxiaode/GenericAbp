namespace Generic.Abp.FileManagement.Files;

public class FileConsts
{
    public const int FilenameMaxLength = 128;
    public const int MimeTypeMaxLength = 128;
    public const int FileTypeMaxLength = 32;
    public const int DescriptionMaxLength = 256;
    public const int HashMaxLength = 32;
    public const int PathMaxLength = 256;

    public const int DefaultChunkSize = 2097152;
    public const long LargeFileSizeThreshold = 1024 * 1024 * 5; // 5 MB

    public static int ProviderNameMaxLength { get; set; } = 16;
    public static int ProviderKeyMaxLength { get; set; } = 64;
}