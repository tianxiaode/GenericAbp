using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathUpdateDto : VirtualPathCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;
}