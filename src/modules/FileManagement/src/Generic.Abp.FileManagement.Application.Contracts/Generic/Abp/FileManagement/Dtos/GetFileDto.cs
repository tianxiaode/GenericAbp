using Generic.Abp.FileManagement.FileInfoBases;
using System;

namespace Generic.Abp.FileManagement.Dtos;

[Serializable]
public class GetFileDto
{
    public int ChunkSize { get; set; } = FileConsts.DefaultChunkSize;
    public int? Index { get; set; } = null;
}