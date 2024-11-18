using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathPermissionManager(
    IVirtualPathPermissionRepository repository,
    IdentityUserManager userManager)
    : FileManagementPermissionManagerBase<VirtualPathPermission, IVirtualPathPermissionRepository>(repository,
        userManager)
{
}