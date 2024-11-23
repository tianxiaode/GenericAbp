using Generic.Abp.FileManagement.FileInfoBases;
using System;
using Generic.Abp.Extensions.Entities.Trees;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources;

public interface IResource : IHasExtraProperties, IMultiTenant, ITree
{
    /// <summary>
    /// 资源类型：File\Folder\VirtualPath
    /// </summary>
    ResourceType Type { get; }

    IFileInfoBase? FileInfoBase { get; }
    Guid? FileInfoBaseId { get; }
    Guid? FolderId { get; }

    /// <summary>
    /// 是否静态资源，由系统自动创建的，不允许删除
    /// </summary>
    bool IsStatic { get; }
}