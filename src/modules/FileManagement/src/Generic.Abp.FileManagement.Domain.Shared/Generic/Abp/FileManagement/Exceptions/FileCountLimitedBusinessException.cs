using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class FileCountLimitedBusinessException : BusinessException
{
    public FileCountLimitedBusinessException(int max)
    {
        Code = FileManagementErrorCodes.FileCountLimitedBusinessException;
        WithData(FileManagementErrorCodes.FileCountLimitedMax, max);
    }
}