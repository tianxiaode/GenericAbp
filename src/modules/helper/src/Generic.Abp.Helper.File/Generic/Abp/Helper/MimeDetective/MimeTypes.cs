﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using JetBrains.Annotations;

namespace Generic.Abp.Helper.MimeDetective
{
    public static class MimeTypes
    {
        // all the file types to be put into one list
        public static List<FileType> Types;

        static MimeTypes()
        {
            Types = new List<FileType>
            {
                PDF, WORD, EXCEL, JPEG, ZIP, RAR, RTF, PNG, PPT, GIF, DLL_EXE, MSDOC,
                BMP, ZIP_7z, ZIP_7z_2, GZ_TGZ, TAR_ZH, TAR_ZV, OGG, ICO, XML, MIDI, FLV, WAVE, DWG, LIB_COFF,
                PST, PSD,
                AES, SKR, SKR_2, PKR, EML_FROM, ELF, TXT_UTF8, TXT_UTF16_BE, TXT_UTF16_LE, TXT_UTF32_BE, TXT_UTF32_LE,
                MP4, CSV
            };
        }

        #region Constants

        // file headers are taken from here:
        //http://www.garykessler.net/library/file_sigs.html
        //mime types are taken from here:
        //http://www.webmaster-toolkit.com/mime-types.shtml

        #region office, excel, ppt and documents, xml, pdf, rtf, msdoc
        // office and documents
        public static readonly FileType WORD = new FileType(new byte?[] { 0xEC, 0xA5, 0xC1, 0x00 }, 512, "doc", "application/msword");
        public static readonly FileType EXCEL = new FileType(new byte?[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 }, 512, "xls", "application/excel");
        public static readonly FileType PPT = new FileType(new byte?[] { 0xFD, 0xFF, 0xFF, 0xFF, null, 0x00, 0x00, 0x00 }, 512, "ppt", "application/mspowerpoint");

        //ms office and openoffice docs (they're zip files: rename and enjoy!)
        //don't add them to the list, as they will be 'subtypes' of the ZIP type
        public static readonly FileType WORDX = new FileType(new byte?[0], 512, "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        public static readonly FileType EXCELX = new FileType(new byte?[0], 512, "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        public static readonly FileType ODT = new FileType(new byte?[0], 512, "odt", "application/vnd.oasis.opendocument.text");
        public static readonly FileType ODS = new FileType(new byte?[0], 512, "ods", "application/vnd.oasis.opendocument.spreadsheet");

        // common documents
        public static readonly FileType RTF = new FileType(new byte?[] { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 }, "rtf", "application/rtf");
        public static readonly FileType PDF = new FileType(new byte?[] { 0x25, 0x50, 0x44, 0x46 }, "pdf", "application/pdf");
        public static readonly FileType MSDOC = new FileType(new byte?[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }, "", "application/octet-stream");
        //application/xml text/xml
        public static readonly FileType XML = new FileType(new byte?[] { 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E },
                                                            "xml,xul", "text/xml");

        //text files
        public static readonly FileType TXT = new FileType(new byte?[0], "txt", "text/plain");
        public static readonly FileType TXT_UTF8 = new FileType(new byte?[] { 0xEF, 0xBB, 0xBF }, "txt", "text/plain");
        public static readonly FileType TXT_UTF16_BE = new FileType(new byte?[] { 0xFE, 0xFF }, "txt", "text/plain");
        public static readonly FileType TXT_UTF16_LE = new FileType(new byte?[] { 0xFF, 0xFE }, "txt", "text/plain");
        public static readonly FileType TXT_UTF32_BE = new FileType(new byte?[] { 0x00, 0x00, 0xFE, 0xFF }, "txt", "text/plain");
        public static readonly FileType TXT_UTF32_LE = new FileType(new byte?[] { 0xFF, 0xFE, 0x00, 0x00 }, "txt", "text/plain");
        public static readonly FileType CSV = new FileType(new byte?[0], "csv", "text/csv");

        #endregion

        // graphics
        #region Graphics jpeg, png, gif, bmp, ico

        public static readonly FileType JPEG = new FileType(new byte?[] { 0xFF, 0xD8, 0xFF }, "jpg", "image/jpeg");
        public static readonly FileType PNG = new FileType(new byte?[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, "png", "image/png");
        public static readonly FileType GIF = new FileType(new byte?[] { 0x47, 0x49, 0x46, 0x38, null, 0x61 }, "gif", "image/gif");
        public static readonly FileType BMP = new FileType(new byte?[] { 66, 77 }, "bmp", "image/gif");
        public static readonly FileType ICO = new FileType(new byte?[] { 0, 0, 1, 0 }, "ico", "image/x-icon");

        #endregion

        //bmp, tiff
        #region Zip, 7zip, rar, dll_exe, tar, bz2, gz_tgz

        public static readonly FileType GZ_TGZ = new FileType(new byte?[] { 0x1F, 0x8B, 0x08 }, "gz, tgz", "application/x-gz");

        public static readonly FileType ZIP_7z = new FileType(new byte?[] { 66, 77 }, "7z", "application/x-compressed");
        public static readonly FileType ZIP_7z_2 = new FileType(new byte?[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, "7z", "application/x-compressed");

        public static readonly FileType ZIP = new FileType(new byte?[] { 0x50, 0x4B, 0x03, 0x04 }, "zip", "application/x-compressed");
        public static readonly FileType RAR = new FileType(new byte?[] { 0x52, 0x61, 0x72, 0x21 }, "rar", "application/x-compressed");
        public static readonly FileType DLL_EXE = new FileType(new byte?[] { 0x4D, 0x5A }, "dll, exe", "application/octet-stream");

        //Compressed tape archive file using standard (Lempel-Ziv-Welch) compression
        public static readonly FileType TAR_ZV = new FileType(new byte?[] { 0x1F, 0x9D }, "tar.z", "application/x-tar");

        //Compressed tape archive file using LZH (Lempel-Ziv-Huffman) compression
        public static readonly FileType TAR_ZH = new FileType(new byte?[] { 0x1F, 0xA0 }, "tar.z", "application/x-tar");

        //bzip2 compressed archive
        public static readonly FileType BZ2 = new FileType(new byte?[] { 0x42, 0x5A, 0x68 }, "bz2,tar,bz2,tbz2,tb2", "application/x-bzip2");


        #endregion


        #region Media ogg, midi, flv, dwg, pst, psd

        // media 
        public static readonly FileType OGG = new FileType(new byte?[] { 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, "oga,ogg,ogv,ogx", "application/ogg");
        //MID, MIDI     Musical Instrument Digital Interface (MIDI) sound file
        public static readonly FileType MIDI = new FileType(new byte?[] { 0x4D, 0x54, 0x68, 0x64 }, "midi,mid", "audio/midi");

        //FLV       Flash video file
        public static readonly FileType FLV = new FileType(new byte?[] { 0x46, 0x4C, 0x56, 0x01 }, "flv", "application/unknown");

        //WAV       Resource Interchange File Format -- Audio for Windows file, where xx xx xx xx is the file size (little endian), audio/wav audio/x-wav

        public static readonly FileType WAVE = new FileType(new byte?[] { 0x52, 0x49, 0x46, 0x46, null, null, null, null,
                                                        0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20  }, "wav", "audio/wav");

        public static readonly FileType PST = new FileType(new byte?[] { 0x21, 0x42, 0x44, 0x4E }, "pst", "application/octet-stream");

        //eneric AutoCAD drawing image/vnd.dwg  image/x-dwg application/acad
        public static readonly FileType DWG = new FileType(new byte?[] { 0x41, 0x43, 0x31, 0x30 }, "dwg", "application/acad");

        //Photoshop image file
        public static readonly FileType PSD = new FileType(new byte?[] { 0x38, 0x42, 0x50, 0x53 }, "psd", "application/octet-stream");

        #endregion

        public static readonly FileType LIB_COFF = new FileType(new byte?[] { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E, 0x0A }, "lib", "application/octet-stream");

        #region Crypto aes, skr, skr_2, pkr

        //AES Crypt file format. (The fourth byte is the version number.)
        public static readonly FileType AES = new FileType(new byte?[] { 0x41, 0x45, 0x53 }, "aes", "application/octet-stream");

        //SKR       PGP secret keyring file
        public static readonly FileType SKR = new FileType(new byte?[] { 0x95, 0x00 }, "skr", "application/octet-stream");

        //SKR       PGP secret keyring file
        public static readonly FileType SKR_2 = new FileType(new byte?[] { 0x95, 0x01 }, "skr", "application/octet-stream");

        //PKR       PGP public keyring file
        public static readonly FileType PKR = new FileType(new byte?[] { 0x99, 0x01 }, "pkr", "application/octet-stream");


        #endregion

        /*
            * 46 72 6F 6D 20 20 20 or       From
        46 72 6F 6D 20 3F 3F 3F or      From ???
        46 72 6F 6D 3A 20       From:
        EML     A commmon file extension for e-mail files. Signatures shown here
        are for Netscape, Eudora, and a generic signature, respectively.
        EML is also used by Outlook Express and QuickMail.
            */
        public static readonly FileType EML_FROM = new FileType(new byte?[] { 0x46, 0x72, 0x6F, 0x6D }, "eml", "message/rfc822");


        //EVTX      Windows Vista event log file
        public static readonly FileType ELF = new FileType(new byte?[] { 0x45, 0x6C, 0x66, 0x46, 0x69, 0x6C, 0x65, 0x00 }, "elf", "text/plain");

        public static readonly FileType MP4 = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70 }, 4, "mp4", "video/mp4");

        public static readonly FileType APK = new FileType(new byte?[] { 0x50, 0x4B, 0x03, 0x04 }, "apk",
            "application/vnd.android.package-archive");

        public static readonly FileType BIN = new FileType(new byte?[] { 0x55, 0x36, 0x41 }, "bin",
            "application/octet-stream");

        // number of bytes we read from a file
        public const int MaxHeaderSize = 560;  // some file formats have headers offset to 512 bytes

        #endregion

        #region Main Methods

        /// <summary>
        /// Read header of bytes and depending on the information in the header
        /// return object FileType.
        /// Return null in case when the file type is not identified. 
        /// Throws Application exception if the file can not be read or does not exist
        /// </summary>
        /// <remarks>
        /// A temp file is written to get a FileInfo from the given bytes.
        /// If this is not intended use 
        /// 
        ///     GetFileType(() => bytes); 
        ///     
        /// </remarks>
        /// <param name="file">The FileInfo object.</param>
        /// <param name="bytes"></param>
        /// <param name="allowFileTypes">允许的文件类型</param>
        /// <returns>FileType or null not identified</returns>
        public static FileType GetFileType(this byte[] bytes, [CanBeNull] List<FileType> allowFileTypes = null)
        {
            return GetFileType(new MemoryStream(bytes), allowFileTypes);
        }

        /// <summary>
        /// Read header of a stream and depending on the information in the header
        /// return object FileType.
        /// Return null in case when the file type is not identified. 
        /// Throws Application exception if the file can not be read or does not exist
        /// </summary>
        /// <param name="file">The FileInfo object.</param>
        /// <param name="stream"></param>
        /// <param name="allowFileTypes">允许的文件类型</param>
        /// <returns>FileType or null not identified</returns>
        public static FileType GetFileType(this Stream stream, [CanBeNull] List<FileType> allowFileTypes = null)
        {
            FileType fileType = null;
            var fileName = Path.GetTempFileName();

            try
            {
                using (var fileStream = File.Create(fileName))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
                fileType = GetFileType(new FileInfo(fileName), allowFileTypes);
            }
            finally
            {
                File.Delete(fileName);
            }
            return fileType;
        }

        /// <summary>
        /// Read header of a file and depending on the information in the header
        /// return object FileType.
        /// Return null in case when the file type is not identified. 
        /// Throws Application exception if the file can not be read or does not exist
        /// </summary>
        /// <param name="file">The FileInfo object.</param>
        /// <param name="allowFileTypes">允许的文件类型</param>
        /// <returns>FileType or null not identified</returns>
        public static FileType GetFileType(this FileInfo file, List<FileType> allowFileTypes = null)
        {
            return GetFileType(() => ReadFileHeader(file, MaxHeaderSize), allowFileTypes, file.FullName);
        }

        /// <summary>
        /// Read header of a file and depending on the information in the header
        /// return object FileType.
        /// Return null in case when the file type is not identified. 
        /// Throws Application exception if the file can not be read or does not exist
        /// </summary>
        /// <param name="fileHeaderReadFunc">A function which returns the bytes found</param>
        /// <param name="allowFileTypes">允许的文件类型</param>
        /// <param name="fileFullName">If given and file typ is a zip file, a check for docx and xlsx is done</param>
        /// <returns>FileType or null not identified</returns>
        public static FileType GetFileType(Func<byte[]> fileHeaderReadFunc, List<FileType> allowFileTypes = null, string fileFullName = "")
        {
            if (allowFileTypes == null) allowFileTypes = MimeTypes.Types;

            // if none of the types match, return null
            FileType fileType = null;

            // read first n-bytes from the file
            byte[] fileHeader = fileHeaderReadFunc();

            // checking if it's binary (not really exact, but should do the job)
            // shouldn't work with UTF-16 OR UTF-32 files
            if (fileHeader.All(b => b != 0))
            {
                fileType = TXT;
                return allowFileTypes.Any(m => m.Extension == TXT.Extension) ? fileType : null;
            }
            else
            {
                // compare the file header to the stored file headers
                foreach (var type in allowFileTypes)
                {
                    int matchingCount = GetFileMatchingCount(fileHeader, type);
                    if (matchingCount == type.Header.Length)
                    {
                        // check for docx and xlsx only if a file name is given
                        // there may be situations where the file name is not given
                        // or it is unpracticable to write a temp file to get the FileInfo
                        //                        if (type.Equals(ZIP) && !String.IsNullOrEmpty(fileFullName))
                        //                            fileType = CheckForDocxAndXlsx(type, fileFullName);
                        //                        else
                        fileType = type;    // if all the bytes match, return the type

                        //break;
                    }
                }
            }
            return fileType;
        }

        /// <summary>
        /// Determines whether provided file belongs to one of the provided list of files
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="requiredTypes">The required types.</param>
        /// <returns>
        ///   <c>true</c> if file of the one of the provided types; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFileOfTypes(this FileInfo file, List<FileType> requiredTypes)
        {
            FileType currentType = file.GetFileType();

            if (null == currentType)
            {
                return false;
            }

            return requiredTypes.Contains(currentType);
        }

        /// <summary>
        /// Determines whether provided file belongs to one of the provided list of files,
        /// where list of files provided by string with Comma-Separated-Values of extensions
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="requiredTypes">The required types.</param>
        /// <returns>
        ///   <c>true</c> if file of the one of the provided types; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFileOfTypes(this FileInfo file, string CSV)
        {
            List<FileType> providedTypes = GetFileTypesByExtensions(CSV);

            return file.IsFileOfTypes(providedTypes);
        }

        /// <summary>
        /// Gets the list of FileTypes based on list of extensions in Comma-Separated-Values string
        /// </summary>
        /// <param name="CSV">The CSV String with extensions</param>
        /// <returns>List of FileTypes</returns>
        private static List<FileType> GetFileTypesByExtensions(string CSV)
        {
            var extensions = CSV.ToUpper().Replace(" ", "").Split(',');

            return Types.Where(type => extensions.Contains(type.Extension.ToUpper())).ToList();
        }

        private static FileType CheckForDocxAndXlsx(FileType type, string fileFullName)
        {
            FileType result = null;

            //check for docx and xlsx
            using (var zipFile = ZipFile.OpenRead(fileFullName))
            {
                if (zipFile.Entries.Any(e => e.FullName.StartsWith("word/")))
                    result = WORDX;
                else if (zipFile.Entries.Any(e => e.FullName.StartsWith("xl/")))
                    result = EXCELX;
                else
                    result = CheckForOdtAndOds(result, zipFile);
            }
            return result;
        }

        private static FileType CheckForOdtAndOds(FileType result, ZipArchive zipFile)
        {
            var ooMimeType = zipFile.Entries.FirstOrDefault(e => e.FullName == "mimetype");
            if (ooMimeType == null) return result;
            using (var textReader = new StreamReader(ooMimeType.Open()))
            {
                var mimeType = textReader.ReadToEnd();
                textReader.Close();

                if (mimeType == ODT.Mime)
                    result = ODT;
                else if (mimeType == ODS.Mime)
                    result = ODS;
            }

            return result;
        }

        private static int GetFileMatchingCount(byte[] fileHeader, FileType type)
        {
            var matchingCount = 0;
            for (var i = 0; i < type.Header.Length; i++)
            {
                // if file offset is not set to zero, we need to take this into account when comparing.
                // if byte in type.header is set to null, means this byte is variable, ignore it
                if (type.Header[i] != null && type.Header[i] != fileHeader[i + type.HeaderOffset])
                {
                    // if one of the bytes does not match, move on to the next type
                    matchingCount = 0;
                    break;
                }
                else
                {
                    matchingCount++;
                }
            }

            return matchingCount;
        }

        /// <summary>
        /// Reads the file header - first (16) bytes from the file
        /// </summary>
        /// <param name="file">The file to work with</param>
        /// <param name="maxHeaderSize"></param>
        /// <returns>Array of bytes</returns>
        private static Byte[] ReadFileHeader(FileInfo file, int maxHeaderSize)
        {
            var header = new byte[maxHeaderSize];
            try  // read file
            {
                using (FileStream fsSource = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    // read first symbols from file into array of bytes.
                    fsSource.Read(header, 0, maxHeaderSize);
                }   // close the file stream

            }
            catch (Exception e) // file could not be found/read
            {
                throw new ApplicationException("Could not read file : " + e.Message);
            }

            return header;
        }
        #endregion

        #region isType functions


        /// <summary>
        /// Determines whether the specified file is of provided type
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="type">The FileType</param>
        /// <returns>
        ///   <c>true</c> if the specified file is type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsType(this FileInfo file, FileType type)
        {
            var actualType = GetFileType(file);

            return null != actualType && actualType.Equals(type);
        }

        /// <summary>
        /// Determines whether the specified file is MS Excel spreadsheet
        /// </summary>
        /// <param name="fileInfo">The FileInfo</param>
        /// <returns>
        ///   <c>true</c> if the specified file info is excel; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExcel(this FileInfo fileInfo)
        {
            return fileInfo.IsType(EXCEL);
        }

        /// <summary>
        /// Determines whether the specified file is Microsoft PowerPoint Presentation
        /// </summary>
        /// <param name="fileInfo">The FileInfo object.</param>
        /// <returns>
        ///   <c>true</c> if the specified file info is PPT; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPpt(this FileInfo fileInfo)
        {
            return fileInfo.IsType(PPT);
        }

        /// <summary>
        /// Checks if the file is executable
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static bool IsExe(this FileInfo fileInfo)
        {
            return fileInfo.IsType(DLL_EXE);
        }

        /// <summary>
        /// Check if the file is Microsoft Installer.
        /// Beware, many Microsoft file types are starting with the same header. 
        /// So use this one with caution. If you think the file is MSI, just need to confirm, use this method. 
        /// But it could be MSWord or MSExcel, or Powerpoint... 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static bool IsMsi(this FileInfo fileInfo)
        {
            // MSI has a generic DOCFILE header. Also it matches PPT files
            return fileInfo.IsType(PPT) || fileInfo.IsType(MSDOC);
        }
        #endregion
    }
}
