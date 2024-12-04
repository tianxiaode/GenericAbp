using System;
using System.Collections.Generic;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathQueryParams : CreationTimeQueryParams, IVirtualPathQueryParams
{
    public bool? IsAccessible { get; set; }

    protected override string DefaultSortingOrder => "desc";

    protected override HashSet<string> AllowedSortingFields { get; set; } =
    [
        "CreationTime",
        "Name",
        "IsAccessible",
        "StartTime",
        "EndTime"
    ];
}