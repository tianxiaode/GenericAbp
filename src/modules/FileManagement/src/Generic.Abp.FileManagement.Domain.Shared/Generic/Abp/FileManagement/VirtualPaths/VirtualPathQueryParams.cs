using System;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathQueryParams : CreationTimeQueryParams, IVirtualPathQueryParams
{
    public bool? IsAccessible { get; set; }

    protected override string DefaultSortingPattern => "{0}CreationTime desc";
}