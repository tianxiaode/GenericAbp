using System.Collections.Generic;

namespace Generic.Abp.Extensions.Entities.Districts;

public class DistrictConsts
{
    public static int PostcodeMaxLength { get; set; } = 16;

    public static int DisplayNameMaxLength { get; set; } = 256;

    public static string RootName = "全部";

    public static string RootCode = "root";

    public static Dictionary<string, string> RootTranslation = new Dictionary<string, string>()
    {
        { "en", "All" },
        { "zh-Hant", "全部" },
    };

    public static string China = "中国";

    public static string ChinaCode = "china";

    public static Dictionary<string, string> ChinaTranslation = new Dictionary<string, string>()
    {
        { "en", "China" },
        { "zh-Hant", "中國" },
    };
}