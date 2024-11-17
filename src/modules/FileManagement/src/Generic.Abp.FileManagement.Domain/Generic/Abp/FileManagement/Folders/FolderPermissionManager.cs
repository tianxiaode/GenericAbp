namespace Generic.Abp.FileManagement.Folders;

public class FolderPermissionManager(IFolderPermissionRepository repository)
    : FileManagementPermissionManagerBase<FolderPermission, IFolderPermissionRepository>(repository)
{
}