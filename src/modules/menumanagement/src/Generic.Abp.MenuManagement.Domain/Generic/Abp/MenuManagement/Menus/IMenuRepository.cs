using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuRepository : IRepository<Menu, Guid>
{
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);
    Task<List<Menu>> GetFilterListAsync(string filter, CancellationToken cancellation = default);
    Task<List<string>> GetCodeListAsync(string filter, CancellationToken cancellation = default);
    Task<List<Menu>> GetListByGroupAsync(string group, CancellationToken cancellation = default);
    Task<List<string>> GetAllGroupNamesAsync(CancellationToken cancellation = default);
}