using System;

namespace Generic.Abp.Extensions.Entities.GetListParams;

public interface IHasCreationTimeSearch
{
    DateTime? StartTime { get; set; }
    DateTime? EndTime { get; set; }
}