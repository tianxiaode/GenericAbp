namespace Generic.Abp.Domain.Entities;

public interface IFullTimeLimit: ITimeLimit
{
    /// <summary>
    /// 长期有效
    /// </summary>
    bool IsLongTerm { get; }

}