using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.Folders;

public class FolderPermissionManager(IFolderPermissionRepository repository, IdentityUserManager userManager)
    : FileManagementPermissionManagerBase<FolderPermission, IFolderPermissionRepository>(repository, userManager)
{
}