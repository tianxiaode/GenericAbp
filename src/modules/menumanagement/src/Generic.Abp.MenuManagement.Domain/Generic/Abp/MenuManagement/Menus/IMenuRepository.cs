using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuRepository : IRepository<Menu, Guid>
{
}