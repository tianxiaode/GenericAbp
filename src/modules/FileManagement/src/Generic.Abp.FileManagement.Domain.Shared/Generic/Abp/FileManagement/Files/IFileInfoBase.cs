namespace Generic.Abp.FileManagement.Files;

public interface IFileInfoBase
{
    /// <summary>
    /// 文件MimeType
    /// </summary>
    string MimeType { get; }

    /// <summary>
    /// 文件类型
    /// </summary>
    string FileType { get; }

    /// <summary>
    /// 文件大小
    /// </summary>
    long Size { get; }
}