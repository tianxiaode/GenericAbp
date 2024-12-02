using System;

namespace Generic.Abp.FileManagement.ExternalShares;

public interface IExternalShareSearchParams
{
    DateTime? StartTime { get; set; }
    DateTime? EndTime { get; set; }
    DateTime? ExpireTimeStart { get; set; }
    DateTime? ExpireTimeEnd { get; set; }
    string? Filter { get; set; }
    public Guid? OwnerId { get; set; }
}