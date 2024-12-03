using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.Extensions.MimeDetective
{
    public static class MimeTypes
    {
        #region Office, Excel, PPT and documents, XML, PDF, RTF, MSDoc

        public static readonly FileType Word = new([0xEC, 0xA5, 0xC1, 0x00], "doc", "application/msword", 512);

        public static readonly FileType WordX = new([0x50, 0x4B, 0x03, 0x04], "docx",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 512);

        public static readonly FileType Excel = new([0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00], "xls",
            "application/excel", 512);

        public static readonly FileType ExcelX = new([0x50, 0x4B, 0x03, 0x04], "xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 512);

        public static readonly FileType Ppt = new([0xFD, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00], "ppt",
            "application/mspowerpoint", 512);

        public static readonly FileType PptX = new([0x50, 0x4B, 0x03, 0x04], "pptx",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation", 512);

        public static readonly FileType Pdf = new([0x25, 0x50, 0x44, 0x46], "pdf", "application/pdf");

        public static readonly FileType Xml =
            new([0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x30, 0x22, 0x3F, 0x3E], "xml", "text/xml");

        #endregion

        #region Graphics JPEG, PNG, GIF, BMP, ICO

        public static readonly FileType Jpg = new([0xFF, 0xD8, 0xFF], "jpg", "image/jpeg");
        public static readonly FileType Png = new([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A], "png", "image/png");
        public static readonly FileType Gif = new([0x47, 0x49, 0x46], "gif", "image/gif");
        public static readonly FileType Bmp = new([0x42, 0x4D], "bmp", "image/bmp");

        #endregion

        #region Video and audio

        public static readonly FileType Mp4 =
            new([0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D], "mp4", "video/mp4");

        public static readonly FileType Mkv = new([0x1A, 0x45, 0xDF, 0xA3], "mkv", "video/x-matroska");
        public static readonly FileType Mp3 = new([0xFF, 0xFB], "mp3", "audio/mpeg");
        public static readonly FileType Wav = new([0x52, 0x49, 0x46, 0x46], "wav", "audio/wav");

        #endregion

        #region Zip, 7zip, RAR, DLL_EXE, TAR, BZ2, GZ_TGZ

        public static readonly FileType Zip = new([0x50, 0x4B, 0x03, 0x04], "zip", "application/x-compressed");

        #endregion

        #region Txt, MD

        public static readonly FileType Txt = new(null, "txt", "text/plain");
        public static readonly FileType Md = new(null, "md", "text/markdown");

        #endregion

        public const int MaxHeaderSize = 560;

        public static readonly List<FileType> Types =
            [Word, WordX, Excel, ExcelX, Ppt, PptX, Jpg, Png, Pdf, Gif, Bmp, Mp4, Mkv, Mp3, Wav, Zip, Txt, Md, Xml];

        public static readonly List<FileType> AudioTypes = [Mp3, Wav];
        public static readonly List<FileType> VideoTypes = [Mp4, Mkv];
        public static readonly List<FileType> ImageTypes = [Jpg, Png, Gif, Bmp];
        public static readonly List<FileType> DocumentTypes = [Word, WordX, Excel, ExcelX, Ppt, PptX, Pdf, Xml];
        public static readonly List<FileType> OfficeTypes = [Word, WordX, Excel, ExcelX, Ppt, PptX];
        public static readonly List<FileType> ArchiveTypes = [Zip];
        public static readonly List<FileType> TextTypes = [Txt, Md];


        // 所有文件类型检查方法合并为一个异步方法
        public static async Task<(bool, string)> IsFileTypeAsync(this byte[] header, List<FileType> allowedTypes,
            string fileName = "")
        {
            try
            {
                // 文件头检测
                var fileType = await GetFileTypeAsync(header, allowedTypes);

                // 无法通过文件头检测时，尝试通过扩展名匹配
                if (fileType != null || string.IsNullOrEmpty(fileName))
                {
                    return (fileType != null, fileType?.Extension ?? string.Empty);
                }

                var extension = Path.GetExtension(fileName)?.TrimStart('.').ToLower();
                fileType = allowedTypes.FirstOrDefault(t =>
                    t.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));

                return (fileType != null, fileType?.Extension ?? string.Empty);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("文件类型识别失败: " + ex.Message);
            }
        }

        public static async Task<(bool, string)> IsFileTypeAsync(this Stream file, List<FileType> allowedTypes,
            string fileName = "", CancellationToken cancellationToken = default)
        {
            var header = await ReadFileHeaderAsync(file, MaxHeaderSize, cancellationToken);

            // 优先尝试文件头和扩展名
            var (isMatched, extension) = await IsFileTypeAsync(header, allowedTypes, fileName);
            if (isMatched)
            {
                return (true, extension);
            }

            // 无扩展名且无法通过文件头识别，默认处理为文本文件
            if (!string.IsNullOrEmpty(fileName))
            {
                return (false, string.Empty);
            }

            var contentType = await AnalyzeContentAsync(header, TextTypes);
            return (true, contentType?.Extension ?? string.Empty);
        }

        public static async Task<FileType?> GetFileTypeAsync(this byte[] header, List<FileType> allowedFileTypes)
        {
            foreach (var fileType in allowedFileTypes)
            {
                if (fileType.Header != null &&
                    await GetFileMatchingCountAsync(header, fileType.Header, fileType.HeaderOffset) ==
                    fileType.Header.Length)
                {
                    return fileType;
                }
            }

            // 检测文本文件类型
            if (allowedFileTypes.Any(t => TextTypes.Contains(t)))
            {
                return await AnalyzeContentAsync(header, TextTypes);
            }

            return null;
        }

        public static Task<List<FileType>> GetFileTypesAsync(this string allowedFileTypes)
        {
            var allow = allowedFileTypes.Split(',').Select(s => s.Trim()).ToList();
            //根据扩展名获取文件类型
            var types = Types.Where(t => allow.Contains(t.Extension)).ToList();
            return Task.FromResult(types);
        }

        private static Task<FileType> AnalyzeContentAsync(byte[] content, List<FileType> textFileTypes)
        {
            var textContent = System.Text.Encoding.UTF8.GetString(content);

            // 如果包含 Markdown 特有符号，判断为 Markdown
            if (textContent.Contains("#") || textContent.Contains("```") || textContent.Contains("- ["))
            {
                return Task.FromResult(Md);
            }

            // 默认判断为纯文本
            return Task.FromResult(Txt);
        }


        private static Task<int> GetFileMatchingCountAsync(byte[] fileHeader, byte[] header, int offset)
        {
            return Task.FromResult(header.Where((t, i) => t != fileHeader[i + offset]).Any()
                ? 0
                : // 直接返回0，避免不必要的循环
                header.Length); // 直接返回匹配的长度
        }

        private static async Task<byte[]> ReadFileHeaderAsync(Stream file, int maxHeaderSize,
            CancellationToken cancellationToken = default)
        {
            var header = new byte[maxHeaderSize];
            try
            {
#if NETSTANDARD2_0
                // 使用老式重载方法（不支持 Memory<byte>）
                var bytesRead = await file.ReadAsync(header, 0, maxHeaderSize, cancellationToken);
#else
                // 使用现代化重载方法（支持 Memory<byte>）
                var memory = header.AsMemory(0, maxHeaderSize);
                var bytesRead = await file.ReadAsync(memory, cancellationToken);
#endif
                return header;
            }
            catch (Exception e)
            {
                throw new ApplicationException("无法读取文件: " + e.Message, e);
            }
        }
    }
}