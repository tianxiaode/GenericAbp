using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.Files;

public class FilePermissionManager(
    IFilePermissionRepository repository,
    IdentityUserManager userManager)
    : FileManagementPermissionManagerBase<FilePermission, IFilePermissionRepository>(repository, userManager)
{
}