using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class InvalidChunkHashBusinessException : BusinessException
{
    public InvalidChunkHashBusinessException(int index, string oldHash, string newHash) : base(FileManagementErrorCodes
        .InvalidChunkHash)
    {
        WithData(FileManagementErrorCodes.InvalidChunkHashIndex, index)
            .WithData(FileManagementErrorCodes.InvalidChunkHashOldHash, oldHash)
            .WithData(FileManagementErrorCodes.InvalidChunkHashNewHash, newHash);
    }
}