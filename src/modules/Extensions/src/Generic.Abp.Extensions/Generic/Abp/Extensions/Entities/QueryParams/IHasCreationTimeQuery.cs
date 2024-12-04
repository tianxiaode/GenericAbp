using System;

namespace Generic.Abp.Extensions.Entities.QueryParams;

public interface IHasCreationTimeQuery
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}