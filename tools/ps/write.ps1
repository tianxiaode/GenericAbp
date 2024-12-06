# 定义 Manifest 文件路径
$manifestPath = "C:\temp\example_manifest.bin"

# 定义文件数据
$hash = "abcdef1234567890abcdef1234567890"
$size = 1048576  # 1MB
$chunkSize = 262144  # 256KB
$filename = "example.txt"
$chunks = @{
    0 = "chunkhash0"
    1 = "chunkhash1"
}

# 打开文件流
$stream = [System.IO.File]::Open($manifestPath, [System.IO.FileMode]::Create, [System.IO.FileAccess]::Write)
$writer = New-Object System.IO.BinaryWriter($stream)

try {
    # 写入文件哈希 (固定 32 字节)
    $hashBytes = [System.Text.Encoding]::UTF8.GetBytes($hash.PadRight(32))
    $writer.Write($hashBytes)

    # 写入文件大小 (8 字节 long)
    $writer.Write([long]$size)

    # 写入分片大小 (4 字节 int)
    $writer.Write([int]$chunkSize)

    # 写入文件名长度和文件名
    $filenameBytes = [System.Text.Encoding]::UTF8.GetBytes($filename)
    $writer.Write([int]$filenameBytes.Length)
    $writer.Write($filenameBytes)

    # 写入分片数量 (4 字节 int)
    $writer.Write([int]$chunks.Count)

    # 写入分片索引和哈希值
    foreach ($chunk in $chunks.GetEnumerator()) {
        $writer.Write([int]$chunk.Key)  # 分片索引
        $chunkHashBytes = [System.Text.Encoding]::UTF8.GetBytes($chunk.Value.PadRight(32))
        $writer.Write($chunkHashBytes)  # 分片哈希
    }

    Write-Host "Manifest 文件写入完成：$manifestPath"
} finally {
    # 关闭流
    $writer.Close()
    $stream.Close()
}
