using System;
using System.Collections.Generic;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceQueryParams : IBaseQueryParams, IHasCreationTimeQuery
{
    string? FileType { get; set; }
    long? MinFileSize { get; set; }
    long? MaxFileSize { get; set; }
    Guid? OwnerId { get; set; }
}

[Serializable]
public class ResourceQueryParams : CreationTimeQueryParams, IResourceQueryParams
{
    public string? FileType { get; set; }
    public long? MinFileSize { get; set; }
    public long? MaxFileSize { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid ParentId { get; set; }

    protected override HashSet<string> AllowedSortingFields { get; set; } =
    [
        "Name",
        "CreationTime",
        "FileExtension",
        "FileSize"
    ];
}