using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathGetListInput : PagedAndSortedResultRequestDto, IVirtualPathSearchParams
{
    public string? Filter { get; set; } = default!;
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public bool? IsAccessible { get; set; } = default!;
}