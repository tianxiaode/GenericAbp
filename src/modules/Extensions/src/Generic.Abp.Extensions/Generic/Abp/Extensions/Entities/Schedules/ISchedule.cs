namespace Generic.Abp.Extensions.Entities.Schedules;

public interface ISchedule
{
    string DisplayName { get; }
    string Minute { get; }
    string Hour { get; }
    string Week { get; }
    string Day { get; }
    string Month { get; }
}