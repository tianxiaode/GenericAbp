using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Generic.Abp.ExportManager.Pdf;

public class PdfExportService : IExportService
{
    public virtual async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, ExportSchema.ExportSchema exportSchema = null,
        CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        var writer = new PdfWriter(memoryStream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);


        foreach (var item in data)
        {
            document.Add(new Paragraph(item?.ToString())); // 这里需要根据你的数据对象来调整
        }

        document.Close();
        return await memoryStream.GetAllBytesAsync(cancellationToken);
    }

    public byte[] GeneratePdf<T>(IEnumerable<T> data, PdfClassMap<T> map)
    {
        using (var memoryStream = new MemoryStream())
        {
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // 添加标题
            map.ConfigureDocument(document);

            // 添加数据
            foreach (var item in data)
            {
                map.MapItem(document, item);
            }

            document.Close();
            return memoryStream.ToArray();
        }
    }
}