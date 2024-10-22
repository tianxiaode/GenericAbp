namespace Generic.Abp.Extensions.Entities.Schedules;

public interface IFullTimeLimit : ITimeLimit
{
    /// <summary>
    /// 长期有效
    /// </summary>
    bool IsLongTerm { get; }
}