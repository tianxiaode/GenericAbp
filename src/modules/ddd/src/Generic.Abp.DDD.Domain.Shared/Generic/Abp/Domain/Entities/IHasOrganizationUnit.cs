using System;

namespace Generic.Abp.Domain.Entities
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