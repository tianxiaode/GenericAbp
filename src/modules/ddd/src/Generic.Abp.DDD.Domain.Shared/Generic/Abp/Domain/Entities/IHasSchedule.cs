using System;

namespace Generic.Abp.Domain.Entities
{
    /// <summary>
    /// 日程接口
    /// </summary>
    public interface IHasSchedule
    {
        Guid ScheduleId { get; }
   }
}