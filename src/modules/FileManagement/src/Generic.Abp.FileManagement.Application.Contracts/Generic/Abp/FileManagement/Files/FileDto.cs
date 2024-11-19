using System;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileDto : ExtensibleAuditedEntityDto, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
    public string Filename { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public long Size { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Hash { get; set; } = default!;
    public bool IsInheritPermissions { get; set; } = true;
}