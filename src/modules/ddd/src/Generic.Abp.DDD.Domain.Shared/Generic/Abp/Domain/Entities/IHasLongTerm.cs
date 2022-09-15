namespace Generic.Abp.Domain.Entities;

public interface IHasLongTerm
{
    /// <summary>
    /// 长期有效
    /// </summary>
    bool IsLongTerm { get; }


}