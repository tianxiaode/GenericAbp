using System;
using System.ComponentModel;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPath : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; set; }

    /// <summary>
    /// 虚拟路径名称
    /// </summary>
    [DisplayName("VirtualPath:Name")]
    public virtual string Name { get; protected set; }

    /// <summary>
    /// 绑定的公共文件夹
    /// </summary>
    [DisplayName("VirtualPath:Resource")]
    public virtual Resource Resource { get; set; }

    /// <summary>
    /// 绑定的公共文件夹Id
    /// </summary>
    [DisplayName("VirtualPath:Resource")]
    public virtual Guid ResourceId { get; protected set; }

    /// <summary>
    /// 是否可访问
    /// </summary>
    [DisplayName("VirtualPath:IsAccessible")]
    public virtual bool IsAccessible { get; protected set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [DisplayName("VirtualPath:StartTime")]
    public virtual DateTime? StartTime { get; protected set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DisplayName("VirtualPath:EndTime")]
    public virtual DateTime? EndTime { get; protected set; }

    public VirtualPath(Guid id, string name, Guid resourceId, bool isAccessible, Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(name, nameof(Name));

        Name = name;
        ResourceId = resourceId;
        IsAccessible = isAccessible;
        TenantId = tenantId;
    }

    public virtual void Rename(string name)
    {
        Check.NotNullOrEmpty(name, nameof(Name));

        Name = name;
    }

    public virtual void ChangeResource(Guid resourceId)
    {
        ResourceId = resourceId;
    }

    public virtual void SetIsAccessible(bool isEnabled)
    {
        IsAccessible = true;
    }

    public virtual void SetDeadline(DateTime? startTime, DateTime? endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}