namespace Generic.Abp.FileManagement.Exceptions;

public class InvalidFileTypeBusinessException: Volo.Abp.BusinessException
{
    public InvalidFileTypeBusinessException()
    {
        Code = FileManagementErrorCodes.InvalidFileType;
    }
}