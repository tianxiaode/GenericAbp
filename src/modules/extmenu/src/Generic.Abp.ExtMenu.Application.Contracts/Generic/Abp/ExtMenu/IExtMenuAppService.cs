using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Generic.Abp.ExtMenu
{
    public interface IExtMenuAppService : IApplicationService
    {
        Task<List<IMenuItemBaseDto>> GetMenusAsync(bool isPhone = false);

    }
}
