using System;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ParticipantIsolationFolders.Dtos;

public class GetParticipantIsolationFileInput : PagedAndSortedResultRequestDto, IResourceQueryParams
{
    public string? Filter { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? FileType { get; set; }
    public long? MinFileSize { get; set; }
    public long? MaxFileSize { get; set; }
    public Guid? OwnerId { get; set; }
}