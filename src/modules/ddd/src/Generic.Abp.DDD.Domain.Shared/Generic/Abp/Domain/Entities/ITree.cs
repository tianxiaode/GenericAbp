using System;

namespace Generic.Abp.Domain.Entities;

public interface ITree
{
    string Code { get; set; }
    Guid? ParentId { get; set;  }
    string DisplayName { get; set;  }

}