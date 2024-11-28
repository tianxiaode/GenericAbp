using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class FolderUpdateDto : FolderCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;
}