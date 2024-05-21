using NPOI.SS.UserModel;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelCellStyle
{
    public bool IsShrinkToFit { get; set; } = false;
    public short DataFormat { get; set; }
    public HorizontalAlignment Alignment { get; set; }
    public bool WrapText { get; set; } = false;
    public VerticalAlignment VerticalAlignment { get; set; }
    public short Rotation { get; set; }
    public short Indention { get; set; }
    public BorderStyle BorderLeft { get; set; }
    public BorderStyle BorderRight { get; set; }
    public BorderStyle BorderTop { get; set; }
    public BorderStyle BorderBottom { get; set; }
    public short LeftBorderColor { get; set; }
    public short RightBorderColor { get; set; }
    public short TopBorderColor { get; set; }
    public short BottomBorderColor { get; set; }
    public FillPattern FillPattern { get; set; } = FillPattern.NoFill;
    public short FillBackgroundColor { get; set; }
    public short FillForegroundColor { get; set; }
    public short BorderDiagonalColor { get; set; }
    public BorderStyle BorderDiagonalLineStyle { get; set; }
    public BorderDiagonal BorderDiagonal { get; set; }
    public ExcelFont Font { get; set; }
    public bool IsLocked { get; set; } = false;
}