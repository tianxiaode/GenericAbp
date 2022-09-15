namespace Generic.Abp.FileManagement.Exceptions;

public class FileChunkErrorBusinessException: Volo.Abp.BusinessException
{
    public FileChunkErrorBusinessException()
    {
        Code = FileManagementErrorCodes.FileChunkError;
    }
}