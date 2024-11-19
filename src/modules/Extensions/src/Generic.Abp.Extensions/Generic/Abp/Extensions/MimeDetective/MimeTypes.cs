using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public const int MaxHeaderSize = 560;

        public static readonly List<FileType> Types =
            [Word, WordX, Excel, ExcelX, Ppt, PptX, Jpg, Png, Pdf, Gif, Bmp, Mp4, Mkv, Mp3, Wav];

        public static readonly List<FileType> AudioTypes = [Mp3, Wav];
        public static readonly List<FileType> VideoTypes = [Mp4, Mkv];
        public static readonly List<FileType> ImageTypes = [Jpg, Png, Gif, Bmp];
        public static readonly List<FileType> DocumentTypes = [Word, WordX, Excel, ExcelX, Ppt, PptX, Pdf, Xml];
        public static readonly List<FileType> OfficeTypes = [Word, WordX, Excel, ExcelX, Ppt, PptX];
        public static readonly List<FileType> ArchiveTypes = [Zip];


        // 所有文件类型检查方法合并为一个异步方法
        public static async Task<(bool, string)> IsFileTypeAsync(this byte[] header, List<FileType> allowedTypes)
        {
            try
            {
                var fileType = await GetFileTypeAsync(header, allowedTypes);
                return (fileType != null, fileType?.Extension ?? string.Empty);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("文件类型识别失败: " + ex.Message);
            }
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

            return null;
        }

        public static Task<List<FileType>> GetFileTypesAsync(this string allowedFileTypes)
        {
            var allow = allowedFileTypes.Split(',').Select(s => s.Trim()).ToList();
            //根据扩展名获取文件类型
            var types = Types.Where(t => allow.Contains(t.Extension)).ToList();
            return Task.FromResult(types);
        }

        private static Task<int> GetFileMatchingCountAsync(byte[] fileHeader, byte[] header, int offset)
        {
            return Task.FromResult(header.Where((t, i) => t != fileHeader[i + offset]).Any()
                ? 0
                : // 直接返回0，避免不必要的循环
                header.Length); // 直接返回匹配的长度
        }

        private static async Task<byte[]> ReadFileHeaderAsync(Stream file, int maxHeaderSize)
        {
            var header = new byte[maxHeaderSize];
            try
            {
                // 直接读取文件头
                await file.ReadAsync(header, 0, maxHeaderSize);
                return header;
            }
            catch (Exception e)
            {
                throw new ApplicationException("无法读取文件: " + e.Message);
            }
        }
    }
}