namespace Generic.Abp.Extensions.MimeDetective
{
    public class FileType(byte[]? header, string extension, string mime, int headerOffset = 0)
    {
        public byte[]? Header { get; set; } = header;
        public int HeaderOffset { get; set; } = headerOffset;
        public string Extension { get; set; } = extension;
        public string Mime { get; set; } = mime;

        public override string ToString()
        {
            return Extension;
        }
    }
}