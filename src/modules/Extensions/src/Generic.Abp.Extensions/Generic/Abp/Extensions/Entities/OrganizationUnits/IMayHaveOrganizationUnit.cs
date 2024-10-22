using System;

namespace Generic.Abp.Extensions.Entities.OrganizationUnits
{
    /// <summary>
    /// 可能带有组织接口
    /// </summary>
    public interface IMayHaveOrganizationUnit
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        Guid? OrganizationUnitId { get; }
    }
}