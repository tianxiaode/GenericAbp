using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

[Serializable]
public class UserFolderUpdateDto : UserFolderCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;
}