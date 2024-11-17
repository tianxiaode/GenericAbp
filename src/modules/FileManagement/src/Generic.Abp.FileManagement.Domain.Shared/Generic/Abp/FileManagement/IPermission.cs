using System;

namespace Generic.Abp.FileManagement;

public interface IPermission
{
    Guid TargetId { get; }
    bool CanRead { get; }
    bool CanWrite { get; }
    bool CanDelete { get; }
    string ProviderName { get; }
    string? ProviderKey { get; }
}