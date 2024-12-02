using System;
using Generic.Abp.Extensions.Entities.GetListParams;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareSearchParams : SearchParams, IHasCreationTimeSearch, IExternalShareSearchParams
{
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public DateTime? ExpireTimeStart { get; set; } = default!;
    public DateTime? ExpireTimeEnd { get; set; } = default!;
}