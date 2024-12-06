using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class OtherUserUploadingTheFileBusinessException()
    : BusinessException(FileManagementErrorCodes.OtherUserUploadingTheFile)
{
}