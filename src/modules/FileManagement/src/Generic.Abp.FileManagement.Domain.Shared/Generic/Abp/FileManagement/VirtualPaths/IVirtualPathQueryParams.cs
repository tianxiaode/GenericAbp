using System;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathQueryParams
{
    DateTime? StartTime { get; set; }
    DateTime? EndTime { get; set; }
    bool? IsAccessible { get; set; }
    string? Filter { get; set; }
}