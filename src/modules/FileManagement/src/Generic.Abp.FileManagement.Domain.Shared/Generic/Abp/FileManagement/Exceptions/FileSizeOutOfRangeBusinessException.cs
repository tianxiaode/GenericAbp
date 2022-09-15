namespace Generic.Abp.FileManagement.Exceptions;

public class FileSizeOutOfRangeBusinessException: Volo.Abp.BusinessException
{
    public FileSizeOutOfRangeBusinessException(long max, object value)
    {
        Code = FileManagementErrorCodes.FileSizeOutOfRange;
        WithData(FileManagementErrorCodes.FileSizeOutOfRangeMax, max)
            .WithData(FileManagementErrorCodes.FileSizeOutOfRangeValue, value);

    }
}