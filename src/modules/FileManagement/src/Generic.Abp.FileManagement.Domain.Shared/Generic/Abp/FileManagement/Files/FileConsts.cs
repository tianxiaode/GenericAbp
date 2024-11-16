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

    public static int ProviderTypeMaxLength { get; set; } = 16;
    public static int ProviderNameMaxLength { get; set; } = 64;
}