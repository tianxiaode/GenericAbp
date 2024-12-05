using System;
using System.Collections.Generic;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.ExternalShares;

[Serializable]
public class ExternalShareQueryParams : CreationTimeQueryParams, IExternalShareQueryParams
{
    public DateTime? ExpireTimeStart { get; set; } = default!;
    public DateTime? ExpireTimeEnd { get; set; } = default!;
    public Guid? OwnerId { get; set; } = default!;

    protected override string DefaultSortingOrder => "desc";

    protected override HashSet<string> AllowedSortingFields { get; set; } =
    [
        "CreationTime",
        "LinkName",
        "ExpireTime"
    ];
}