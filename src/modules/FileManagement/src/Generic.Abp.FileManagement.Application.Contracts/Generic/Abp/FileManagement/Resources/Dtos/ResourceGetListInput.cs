using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourceGetListInput : PagedAndSortedResultRequestDto, IResourceQueryParams
{
    public string? Filter { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? FileType { get; set; }
    public long? MinFileSize { get; set; }
    public long? MaxFileSize { get; set; }
    public Guid? OwnerId { get; set; }
    public ResourceType? ResourceType { get; set; }
}