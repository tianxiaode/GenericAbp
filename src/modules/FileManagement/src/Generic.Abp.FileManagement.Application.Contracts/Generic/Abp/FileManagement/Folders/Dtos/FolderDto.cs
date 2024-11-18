using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; protected set; } = default!;
    public string AllowedFileTypes { get; set; } = default!;
    public FolderDto? Parent { get; set; } = default;
    public bool IsInheritPermissions { get; set; } = true;
    public long StorageQuota { get; set; } = 0;
    public long UsedStorage { get; set; } = 0;
    public long MaxFileSize { get; set; } = 0;
    public string ConcurrencyStamp { get; set; } = default!;
    public bool Leaf { get; set; } = true;

    public FolderDto()
    {
    }

    public FolderDto(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
        Leaf = false;
    }
}