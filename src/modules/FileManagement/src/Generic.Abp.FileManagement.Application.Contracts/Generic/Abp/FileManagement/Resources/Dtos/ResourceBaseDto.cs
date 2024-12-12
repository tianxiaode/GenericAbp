using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourceBaseDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string ConcurrencyStamp { get; set; } = default!;
    public ResourceType Type { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid? OwnerId { get; set; } = default!;
    public bool IsStatic { get; set; } = false;
    public bool IsAccessible { get; set; } = false;
    public string? FileExtension { get; set; } = default!;
    public long? FileSize { get; set; } = default!;
    public string? AllowedFileTypes { get; set; } = default!;
    public long? StorageQuota { get; set; } = default!;
    public long? UsedStorage { get; set; } = default!;
    public long? MaxFileSize { get; set; } = default!;
    public int? AllowedFileCount { get; set; } = default!;

    public ResourceBaseDto()
    {
    }

    public ResourceBaseDto(Guid id, string name, string code)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}