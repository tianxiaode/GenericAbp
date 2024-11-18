namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathPermissionManager(
    IVirtualPathPermissionRepository repository)
    : FileManagementPermissionManagerBase<VirtualPathPermission, IVirtualPathPermissionRepository>(repository)
{
}