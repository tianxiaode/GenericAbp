using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class InvalidChunkIndexBusinessException : BusinessException
{
    public InvalidChunkIndexBusinessException(int totalChunks, int index)
    {
        Code = FileManagementErrorCodes.InvalidChunkIndex;
        WithData(FileManagementErrorCodes.InvalidChunkIndexIndex, index)
            .WithData(FileManagementErrorCodes.InvalidChunkIndexTotal, totalChunks);
    }
}