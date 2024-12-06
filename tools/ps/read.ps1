# 定义 Manifest 文件路径
$manifestPath = "C:\temp\example_manifest.bin"

# 打开文件流
$stream = [System.IO.File]::OpenRead($manifestPath)
$reader = New-Object System.IO.BinaryReader($stream)

try {
    # 读取文件哈希 (固定 32 字节)
    $hashBytes = $reader.ReadBytes(32)
    $hash = [System.Text.Encoding]::UTF8.GetString($hashBytes).Trim()

    # 读取文件大小 (8 字节 long)
    $size = $reader.ReadInt64()

    # 读取分片大小 (4 字节 int)
    $chunkSize = $reader.ReadInt32()

    # 读取文件名
    $filenameLength = $reader.ReadInt32()  # 文件名长度 (4 字节 int)
    $filenameBytes = $reader.ReadBytes($filenameLength)
    $filename = [System.Text.Encoding]::UTF8.GetString($filenameBytes)

    # 读取分片数量 (4 字节 int)
    $chunkCount = $reader.ReadInt32()

    # 读取分片索引和哈希值
    $chunks = @{}
    for ($i = 0; $i -lt $chunkCount; $i++) {
        $chunkIndex = $reader.ReadInt32()  # 分片索引 (4 字节 int)
        $chunkHashBytes = $reader.ReadBytes(32)  # 分片哈希值 (32 字节)
        $chunkHash = [System.Text.Encoding]::UTF8.GetString($chunkHashBytes).Trim()

        $chunks[$chunkIndex] = $chunkHash
    }

    # 输出读取结果
    Write-Host "Hash: $hash"
    Write-Host "Size: $size"
    Write-Host "ChunkSize: $chunkSize"
    Write-Host "Filename: $filename"
    Write-Host "Chunks:"
    $chunks.GetEnumerator() | ForEach-Object {
        Write-Host "  Chunk $_.Key = $_.Value"
    }
} finally {
    # 关闭流
    $reader.Close()
    $stream.Close()
}
