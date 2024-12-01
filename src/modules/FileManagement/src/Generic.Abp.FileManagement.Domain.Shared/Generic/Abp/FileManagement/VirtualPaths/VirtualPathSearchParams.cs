using System;
using Generic.Abp.Extensions.Entities.GetListParams;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathSearchParams : SearchParams, IHasCreationTimeSearch, IVirtualPathSearchParams
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IsAccessible { get; set; }
}