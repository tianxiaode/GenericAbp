﻿using System;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class GetExternalShareResourcesDto : PagedAndSortedResultRequestDto, IResourceQueryParams
{
    public Guid? ResourceId { get; set; }
    public string Token { get; set; } = default!;
    public string? Filter { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? FileType { get; set; }
    public long? MinFileSize { get; set; }
    public long? MaxFileSize { get; set; }
    public Guid? OwnerId { get; set; }
}