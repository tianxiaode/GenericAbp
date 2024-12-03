using System;

namespace Generic.Abp.Extensions.Entities.SearchParams;

public interface IHasCreationTimeSearch
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}