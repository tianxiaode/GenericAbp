namespace Generic.Abp.FileManagement.Files;

public interface IFile: IHasHash
{
    /// <summary>
    /// 文件名
    /// </summary>
    string Filename { get;}

    /// <summary>
    /// 文件MimeType
    /// </summary>
    string MimeType { get;  }

    /// <summary>
    /// 文件类型
    /// </summary>
    string FileType { get;  }

    /// <summary>
    /// 文件大小
    /// </summary>
    long Size { get;  }

    /// <summary>
    /// 文件描述
    /// </summary>
    string  Description { get;  }

    string Path { get; } 

}