using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class FolderConfigurationNotSetBusinessException() : BusinessException(FileManagementErrorCodes
    .FolderConfigurationNotSetBusinessException);