using System;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareSearchParams : CreationTimeQueryParams, IExternalShareSearchParams
{
    public DateTime? ExpireTimeStart { get; set; } = default!;
    public DateTime? ExpireTimeEnd { get; set; } = default!;
    public Guid? OwnerId { get; set; } = default!;
}