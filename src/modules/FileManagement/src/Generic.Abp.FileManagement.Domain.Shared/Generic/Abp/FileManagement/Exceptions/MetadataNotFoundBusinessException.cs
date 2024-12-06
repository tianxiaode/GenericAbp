using Volo.Abp;

namespace Generic.Abp.FileManagement.Exceptions;

public class MetadataNotFoundBusinessException : BusinessException
{
    public MetadataNotFoundBusinessException(string hash) : base(FileManagementErrorCodes.MetadataNotFound)
    {
        WithData(FileManagementErrorCodes.MetadataNotFoundHash, hash);
    }
}