using System;
using Generic.Abp.Extensions.Entities.GetListParams;

namespace Generic.Abp.FileManagement.Resources;

public class ResourceSearchAndPagedAndSortedParams : GetListParams, IHasCreationTimeSearch
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? FileType { get; set; }
}