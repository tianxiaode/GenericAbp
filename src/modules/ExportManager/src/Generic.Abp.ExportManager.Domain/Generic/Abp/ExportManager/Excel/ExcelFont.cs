using NPOI.SS.UserModel;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelFont
{
    public string FontName { get; set; }
    public double FontHeight { get; set; }
    public short FontHeightInPoints { get; set; }
    public bool IsItalic { get; set; }
    public bool IsStrikeout { get; set; }
    public short Color { get; set; }
    public FontSuperScript TypeOffset { get; set; }
    public FontUnderlineType Underline { get; set; }
    public short Charset { get; set; }
    public short BoldWeight { get; set; }
    public bool IsBold { get; set; }
}