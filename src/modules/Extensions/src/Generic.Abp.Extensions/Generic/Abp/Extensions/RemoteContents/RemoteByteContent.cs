namespace Generic.Abp.Extensions.RemoteContents;

public class RemoteByteContent : IRemoteContent
{
    public byte[] Data { get; }
    public string? FileName { get; }
    public string ContentType { get; } = "application/octet-stream";
    public long? ContentLength { get; }

    public RemoteByteContent(byte[] data, string? fileName = null, string? contentType = null,
        long? contentLength = null)
    {
        Data = data;
        FileName = fileName;
        if (contentType != null)
        {
            ContentType = contentType;
        }

        ContentLength = contentLength ?? (long?)data.Length;
    }
}