using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = default!;
}