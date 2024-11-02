using System;

namespace Generic.Abp.Extensions.Entities.Trees;

public interface ITree
{
    public string Code { get; }
    public Guid? ParentId { get; }
    public string Name { get; }
    public void MoveTo(Guid? parentId);
    public void SetCode(string code);
}