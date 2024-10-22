using System;

namespace Generic.Abp.Extensions.Entities.OrganizationUnits
{
    /// <summary>
    /// 组织接口
    /// </summary>
    public interface IHasOrganizationUnit
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        Guid OrganizationUnitId { get; }
    }
}