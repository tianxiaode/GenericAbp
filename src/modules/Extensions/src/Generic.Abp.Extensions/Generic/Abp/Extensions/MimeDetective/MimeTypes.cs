using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Generic.Abp.Extensions.MimeDetective
{
    public static class MimeTypes
    {
        // file headers are taken from here:
        //http://www.garykessler.net/library/file_sigs.html
        //mime types are taken from here:
        //http://www.webmaster-toolkit.com/mime-types.shtml

        #region office, excel, ppt and documents, xml, pdf, rtf, msdoc

        // office and documents
        public static readonly FileType Word = new([0xEC, 0xA5, 0xC1, 0x00], "doc", "application/msword", 512);

        public static readonly FileType Excel = new([0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00], "xls",
            "application/excel", 512);

        public static readonly FileType Ppt = new([0xFD, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00], "ppt",
            "application/mspowerpoint", 512);

        //ms office and openoffice docs (they're zip files: rename and enjoy!)
        //don't add them to the list, as they will be 'subtypes' of the ZIP type
        //public static readonly FileType WordX = new([0xFD, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00], "docx",
        //    "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 512);

        //public static readonly FileType ExcelX = new([0xFD, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00], "xlsx",
        //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 512);

        public static readonly FileType Pdf = new([0x25, 0x50, 0x44, 0x46], "pdf", "application/pdf");

        public static readonly FileType Xml =
            new([0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E], "xml", "text/xml");

        //public static readonly FileType Txt = new FileType(null, "txt", "text/plain");
        public static readonly FileType TxtUtf8 = new([0xEF, 0xBB, 0xBF], "txt", "text/plain");
        public static readonly FileType TxtUtf16Be = new([0xFE, 0xFF], "txt", "text/plain");
        public static readonly FileType TxtUtf16Le = new([0xFF, 0xFE], "txt", "text/plain");
        public static readonly FileType TxtUtf32Be = new([0x00, 0x00, 0xFE, 0xFF], "txt", "text/plain");
        public static readonly FileType TxtUtf32Le = new([0xFF, 0xFE, 0x00, 0x00], "txt", "text/plain");
        public static readonly FileType Csv = new(null, "csv", "text/csv");

        #endregion

        // graphics

        #region Graphics jpeg, png, gif, bmp, ico

        public static readonly FileType Jpg = new([0xFF, 0xD8, 0xFF], "jpg", "image/jpeg");
        public static readonly FileType Jpeg = new([0xFF, 0xD8, 0xFF], "jpeg", "image/jpeg");

        public static readonly FileType Png = new([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A], "png", "image/png");

        #endregion

        //bmp, tiff

        #region Zip, 7zip, rar, dll_exe, tar, bz2, gz_tgz

        public static readonly FileType Zip = new([0x50, 0x4B, 0x03, 0x04], "zip", "application/x-compressed");

        #endregion


        public const int MaxHeaderSize = 560; // some file formats have headers offset to 512 bytes
        public static readonly List<FileType> Types = [Excel, Jpeg, Jpg, Png, Pdf];


        //public static async Task<FileType?> GetFileType(this byte[] bytes, List<FileType> allowFileTypes)
        //{
        //    return await GetFileType(new MemoryStream(bytes), allowFileTypes);
        //}

        public static async Task<FileType?> GetFileType(this byte[] header, List<FileType> allowFileTypes)
        {
            FileType? fileType = null;

            foreach (var allowFileType in allowFileTypes)
            {
                if (allowFileType.Header == null)
                {
                    continue;
                }

                var matchingCount = GetFileMatchingCount(header, allowFileType.Header, allowFileType.HeaderOffset);
                if (matchingCount != allowFileType.Header.Length)
                {
                    continue;
                }

                fileType = allowFileType;
                break;
            }

            return await Task.FromResult(fileType);
        }


        private static int GetFileMatchingCount(byte[] fileHeader, byte[] header, int offset)
        {
            var matchingCount = 0;
            for (var i = 0; i < header.Length; i++)
            {
                // if file offset is not set to zero, we need to take this into account when comparing.
                // if byte in type.header is set to null, means this byte is variable, ignore it
                if (header[i] != fileHeader[i + offset])
                {
                    // if one of the bytes does not match, move on to the next type
                    matchingCount = 0;
                    break;
                }

                matchingCount++;
            }

            return matchingCount;
        }

        private static async Task<byte[]> ReadFileHeaderAsync(Stream file, int maxHeaderSize)
        {
            var header = new byte[maxHeaderSize];
            try // read file
            {
                var test = new MemoryStream();
                await file.CopyToAsync(test);
                _ = await test.ReadAsync(header, 0, maxHeaderSize);
            }
            catch (Exception e) // file could not be found/read
            {
                throw new ApplicationException("Could not read file : " + e.Message);
            }

            return header;
        }
    }
}