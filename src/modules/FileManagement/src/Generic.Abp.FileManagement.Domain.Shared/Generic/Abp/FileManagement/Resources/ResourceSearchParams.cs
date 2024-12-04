using System;

namespace Generic.Abp.FileManagement.Resources;

public class ResourceSearchParams : SearchParams, IHasCreationTimeSearch
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? FileType { get; set; }
    public Guid? OwnerId { get; set; }
}