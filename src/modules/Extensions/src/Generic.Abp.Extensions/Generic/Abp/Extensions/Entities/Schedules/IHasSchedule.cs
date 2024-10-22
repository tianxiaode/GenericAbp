using System;

namespace Generic.Abp.Extensions.Entities.Schedules
{
    /// <summary>
    /// 日程接口
    /// </summary>
    public interface IHasSchedule
    {
        Guid ScheduleId { get; }
    }
}