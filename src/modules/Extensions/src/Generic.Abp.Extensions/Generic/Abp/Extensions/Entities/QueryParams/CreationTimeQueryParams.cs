using System;

namespace Generic.Abp.Extensions.Entities.QueryParams;

public class CreationTimeQueryParams : BaseQueryParams, IHasCreationTimeQuery
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}