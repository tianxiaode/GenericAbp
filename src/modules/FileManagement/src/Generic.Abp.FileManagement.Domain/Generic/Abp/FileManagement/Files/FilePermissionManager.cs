namespace Generic.Abp.FileManagement.Files;

public class FilePermissionManager(IFilePermissionRepository repository)
    : FileManagementPermissionManagerBase<FilePermission, IFilePermissionRepository>(repository)
{
}