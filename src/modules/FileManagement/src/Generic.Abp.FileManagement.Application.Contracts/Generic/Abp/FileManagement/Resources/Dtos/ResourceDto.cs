using System;
using System.ComponentModel;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourceDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; protected set; } = default!;
    public string AllowedFileTypes { get; set; } = default!;
    public ResourceDto? Parent { get; set; } = default;
    public string Quota { get; set; } = default!;
    public string Used { get; set; } = default!;
    public string FileMaxSize { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
    public bool Leaf { get; set; } = true;
    public Guid? FolderId { get; set; } = default!;
    public bool IsStatic { get; set; } = false;
    public Guid? FileInfoBaseId { get; set; } = default!;
    public string? MimeType { get; set; } = default!;
    public string? Extension { get; set; } = default!;
    public long Size { get; set; } = default!;
    public string? Hash { get; set; } = default!;

    public ResourceDto()
    {
    }

    public ResourceDto(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
        Leaf = false;
    }
}