using System;

namespace Generic.Abp.Extensions.Entities.QueryParams;

[Serializable]
public abstract class CreationTimeQueryParams : BaseQueryParams, IHasCreationTimeQuery
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}