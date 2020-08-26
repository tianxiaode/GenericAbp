using System.Collections.Generic;

namespace Generic.Abp.ExtMenu
{
    public interface IMenuItemBase : IMenuItemBaseDto
    {

        List<string> RequiredPermissionNames { get; set; }
    }
}
