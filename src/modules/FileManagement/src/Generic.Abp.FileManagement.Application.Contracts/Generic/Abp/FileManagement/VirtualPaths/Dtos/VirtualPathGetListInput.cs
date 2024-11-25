using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = default!;
}