using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable]
public class FileGetListInput : PagedAndSortedResultRequestDto
{
    public Guid? FolderId { get; set; } = default!;
    public string? Filter { get; set; } = default!;
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public string? FileTypes { get; set; } = default!;
    public long? MinSize { get; set; } = default!;
    public long? MaxSize { get; set; } = default!;
}