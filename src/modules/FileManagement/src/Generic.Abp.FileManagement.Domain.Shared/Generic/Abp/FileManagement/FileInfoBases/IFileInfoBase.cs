using System;

namespace Generic.Abp.FileManagement.FileInfoBases;

public interface IFileInfoBase : IHasHash
{
    Guid Id { get; }

    /// <summary>
    /// 文件MimeType
    /// </summary>
    string MimeType { get; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    string Extension { get; }

    /// <summary>
    /// 文件大小
    /// </summary>
    long Size { get; }

    /// <summary>
    /// 文件路径
    /// </summary>
    string Path { get; }

    /// <summary>
    /// 是否保留文件
    /// </summary>
    FileRetentionPolicy RetentionPolicy { get; }

    /// <summary>
    /// 文件过期时间,删除资源后，如果该文件再没有其他资源引用，则设置该文件过期时间，等待系统自动删除
    /// </summary>
    DateTime? ExpireAt { get; }
}