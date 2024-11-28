using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class OnlyMovePublicFolderBusinessException() : BusinessException(FileManagementErrorCodes.OnlyMovePublicFolder);