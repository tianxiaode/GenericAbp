namespace Generic.Abp.FileManagement.Exceptions;

public class InsufficientStorageSpaceBusinessException : Volo.Abp.BusinessException
{
    public InsufficientStorageSpaceBusinessException(long value, long used, long max)
    {
        Code = FileManagementErrorCodes.FileChunkError;
        WithData(FileManagementErrorCodes.InsufficientStorageSpaceMax, max)
            .WithData(FileManagementErrorCodes.InsufficientStorageSpaceUsed, used)
            .WithData(FileManagementErrorCodes.InsufficientStorageSpaceValue, value);
    }
}