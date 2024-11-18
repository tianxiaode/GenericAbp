using System;

namespace Generic.Abp.Extensions.RemoteContents;

public interface IRemoteContent
{
    string? FileName { get; }
    string ContentType { get; }
    long? ContentLength { get; }
}