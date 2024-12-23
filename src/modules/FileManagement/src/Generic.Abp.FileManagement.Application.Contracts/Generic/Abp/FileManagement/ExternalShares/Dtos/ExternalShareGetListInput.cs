﻿using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

public class ExternalShareGetListInput : PagedAndSortedResultRequestDto, IExternalShareQueryParams
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? ExpireTimeStart { get; set; }
    public DateTime? ExpireTimeEnd { get; set; }
    public string? Filter { get; set; }
    public Guid? OwnerId { get; set; }
}