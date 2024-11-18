using System;
using System.IO;

namespace Generic.Abp.Extensions.RemoteContents;

public class RemoteStreamContent : IRemoteContent, IDisposable
{
    private readonly Stream _stream;
    private readonly bool _disposeStream;
    private bool _disposed;

    public string? FileName { get; }
    public string ContentType { get; } = "application/octet-stream";
    public long? ContentLength { get; }

    public RemoteStreamContent(Stream stream, string? fileName = null, string? contentType = null,
        long? contentLength = null, bool disposeStream = true)
    {
        _stream = stream;
        FileName = fileName;
        if (contentType != null)
            ContentType = contentType;
        ContentLength = contentLength ?? (stream.CanSeek ? new long?(stream.Length - stream.Position) : new long?());
        _disposeStream = disposeStream;
    }

    public Stream GetStream() => _stream;

    public void Dispose()
    {
        if (_disposed || !_disposeStream)
        {
            return;
        }

        _disposed = true;
        _stream?.Dispose();
    }
}