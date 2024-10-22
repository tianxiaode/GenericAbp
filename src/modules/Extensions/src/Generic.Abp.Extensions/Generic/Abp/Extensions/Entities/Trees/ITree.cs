using System;

namespace Generic.Abp.Extensions.Entities.Trees;

public interface ITree
{
    public string Code { get; set; }
    public Guid? ParentId { get; set; }
    public string DisplayName { get; set; }
}