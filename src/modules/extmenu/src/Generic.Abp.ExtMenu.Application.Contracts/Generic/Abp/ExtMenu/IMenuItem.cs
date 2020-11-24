using System.Collections.Generic;

namespace Generic.Abp.ExtMenu
{
    public interface IMenuItem : IMenuItemBase
    {
        public List<string> RequiredPermissionNames { get; set; }
    }
}