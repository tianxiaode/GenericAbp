using System;
using Generic.Abp.Extensions.Entities.Schedules;

namespace Generic.Abp.Extensions.Extensions;

public static class ScheduleExtensions
{
    public static bool IsInSchedule(this ISchedule schedule, DateTime today)
    {
        var minute = today.Minute;
        var hour = today.Hour;
        var day = today.Day;
        var week = (int)today.DayOfWeek;
        var month = today.Month;
        return schedule.Minute.Substring(minute, 1) == "1"
               && schedule.Hour.Substring(hour, 1) == "1"
               && schedule.Day.Substring(day - 1, 1) == "1"
               && schedule.Week.Substring(week, 1) == "1"
               && schedule.Month.Substring(month - 1, 1) == "1";
    }
}