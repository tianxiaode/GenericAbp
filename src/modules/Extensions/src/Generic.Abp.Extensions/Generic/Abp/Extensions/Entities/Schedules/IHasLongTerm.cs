namespace Generic.Abp.Extensions.Entities.Schedules;

public interface IHasLongTerm
{
    /// <summary>
    /// 长期有效
    /// </summary>
    bool IsLongTerm { get; }
}