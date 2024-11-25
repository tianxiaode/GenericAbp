using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class FileCleanupSettingChangeEto() : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public bool DefaultFileUpdateEnabledChanged { get; set; }
    public bool DefaultFileCleanupEnabledChanged { get; set; }
    public bool TemporaryFileUpdateEnabledChanged { get; set; }
    public bool TemporaryFileCleanupEnabledChanged { get; set; }
}