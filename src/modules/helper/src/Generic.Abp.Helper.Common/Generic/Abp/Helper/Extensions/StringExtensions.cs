using System.Linq;
using System.Text;

namespace Generic.Abp.Helper.Extensions
{
    public static class StringExtensions
    {
        public static string UnCapitalize(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length < 1) return str;
            return str.First().ToString().ToLowerInvariant() + str[1..];
        }

        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length < 1) return str;
            return str.First().ToString().ToUpperInvariant() + str[1..];
        }

        public static bool IsAscii(this string str)
        {
            return Encoding.UTF8.GetByteCount(str) == str.Length;
        }


    }
}