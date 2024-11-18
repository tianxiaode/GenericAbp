using System;
using Generic.Abp.FileManagement.Files;

namespace Generic.Abp.FileManagement.Dtos;

[Serializable]
public class GetFileDto
{
    public int ChunkSize { get; set; } = FileConsts.DefaultChunkSize;
    public int Index { get; set; } = 0;
}