using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class StaticFolderCanNotBeMoveOrDeletedBusinessException()
    : BusinessException(FileManagementErrorCodes.StaticFolderCanNotBeMoveOrDeleted);