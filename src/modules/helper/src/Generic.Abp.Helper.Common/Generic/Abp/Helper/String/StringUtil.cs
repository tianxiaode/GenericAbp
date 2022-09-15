using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Generic.Abp.Helper.String
{
    public static class StringUtil
    {
                private static readonly Random Random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmmopqrstuvwxyz0123456789";

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArrayToHexString(byte[]? bytes, bool toLower= true)
        {
            var hexString = string.Empty;
            if (bytes == null) return hexString;
            var strB = new StringBuilder ();

            foreach (var t in bytes)
            {
                strB.Append(t.ToString(toLower ? "x2" : "X2"));
            }                

            hexString = strB.ToString ();
            return hexString;
        }



        #region MyRegion

        public static string GetString(string s, int length, string endStr="")
        {
            var temp = s[..((s.Length < length) ? s.Length : length)];

            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= length)
            {
                return temp;
            }
            for (var i = temp.Length; i >= 0; i--)
            {
                temp = temp[..i];
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= length - endStr.Length)
                {
                    return temp + endStr;
                }
            }
            return endStr;
        }

        #endregion


    }
}