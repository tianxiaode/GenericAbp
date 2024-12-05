using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class InvalidChunkSizeBusinessException : BusinessException
{
    public InvalidChunkSizeBusinessException(long min, long max, long value) : base(FileManagementErrorCodes
        .InvalidChunkSize)
    {
        WithData(FileManagementErrorCodes.InvalidChunkSizeMax, max)
            .WithData(FileManagementErrorCodes.InvalidChunkSizeMin, min)
            .WithData(FileManagementErrorCodes.InvalidChunkSizeValue, value);
    }
}