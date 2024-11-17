using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuRepository : IRepository<Menu, Guid>
{
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);


    Task<List<string>> GetAllCodesByFilterAsync(string filter, string? groupName,
        CancellationToken cancellationToken = default);
}