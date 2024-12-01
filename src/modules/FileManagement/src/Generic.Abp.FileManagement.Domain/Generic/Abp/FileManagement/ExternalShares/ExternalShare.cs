using System;
using System.ComponentModel;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShare : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; set; }

    /// <summary>
    /// 共享名称
    /// </summary>
    [DisplayName("ExternalShare:LinkName")]
    public virtual string LinkName { get; protected set; }

    /// <summary>
    /// 共享的资源
    /// </summary>
    [DisplayName("ExternalShare:Resource")]
    public virtual Resource Resource { get; set; }

    /// <summary>
    /// 共享的资源Id
    /// </summary>
    [DisplayName("ExternalShare:Resource")]
    public virtual Guid ResourceId { get; protected set; }

    /// <summary>
    /// 共享的资源密码
    /// </summary>
    [DisplayName("ExternalShare:Password")]

    public virtual string Password { get; protected set; }

    /// <summary>
    /// 共享的资源过期时间
    /// </summary>
    [DisplayName("ExternalShare:ExpireTime")]
    public virtual DateTime ExpireTime { get; protected set; }

    public ExternalShare(Guid id, Guid resourceId, int days, Guid? tenantId = null) : base(id)
    {
        ResourceId = resourceId;
        TenantId = tenantId;
        LinkName = Guid.NewGuid().ToString("N");
        Password = StringExtensions.GenerateRandomString(4);
        ExpireTime = DateTime.Now.AddDays(days);
    }
}