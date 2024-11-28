using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class OnlyMoveMaxFilesAndFoldersInOnTimeBusinessException : BusinessException
{
    public OnlyMoveMaxFilesAndFoldersInOnTimeBusinessException(int maxCount, long count)
    {
        Code = FileManagementErrorCodes.OnlyMoveMaxFilesAndFoldersInOnTime;
        WithData(FileManagementErrorCodes.OnlyMoveMaxFilesAndFoldersInOnTimeMax, maxCount)
            .WithData(FileManagementErrorCodes.OnlyMoveMaxFilesAndFoldersInOnTimeCount, count);
    }
}