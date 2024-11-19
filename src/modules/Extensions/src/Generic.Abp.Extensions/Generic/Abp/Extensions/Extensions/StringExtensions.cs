using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generic.Abp.Extensions.Extensions
{
    public static class StringExtensions
    {
        public static string UnCapitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length < 1)
            {
                return str;
            }

            return str.First().ToString().ToLowerInvariant() + str.Substring(1);
        }

        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length < 1)
            {
                return str;
            }

            return str.First().ToString().ToUpperInvariant() + str.Substring(1);
        }

        public static bool IsAscii(this string str)
        {
            return Encoding.UTF8.GetByteCount(str) == str.Length;
        }

        public static long ParseToBytes(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(str));
            }

            // 移除所有空白字符
            str = str.Replace(" ", "").ToUpper();

            // 定义单位及其对应的乘数
            var units = new Dictionary<string, double>
            {
                { "K", Math.Pow(1024, 1) },
                { "M", Math.Pow(1024, 2) },
                { "G", Math.Pow(1024, 3) },
                { "T", Math.Pow(1024, 4) },
                { "P", Math.Pow(1024, 5) },
                { "E", Math.Pow(1024, 6) },
                { "Z", Math.Pow(1024, 7) },
                { "Y", Math.Pow(1024, 8) },
                { "KB", Math.Pow(1024, 1) },
                { "MB", Math.Pow(1024, 2) },
                { "GB", Math.Pow(1024, 3) },
                { "TB", Math.Pow(1024, 4) },
                { "PB", Math.Pow(1024, 5) },
                { "EB", Math.Pow(1024, 6) },
                { "ZB", Math.Pow(1024, 7) },
                { "YB", Math.Pow(1024, 8) }
            };

            // 尝试解析数字部分
            var unitPart = "";
            var index = 0;

            while (index < str.Length && char.IsDigit(str[index]) || str[index] == '.')
            {
                index++;
            }

            if (index == 0 || index >= str.Length)
            {
                throw new FormatException("Invalid format. Expected a number followed by an optional unit.");
            }

            var numericPart = double.Parse(str.Substring(0, index));

            if (index < str.Length)
            {
                unitPart = str.Substring(index);
            }

            if (!string.IsNullOrEmpty(unitPart) && units.ContainsKey(unitPart))
            {
                return (long)(numericPart * units[unitPart.ToUpper()]);
            }

            // 如果没有单位，默认为字节
            if (string.IsNullOrEmpty(unitPart))
            {
                return (long)numericPart; // 没有单位，直接返回数字部分
            }

            throw new ArgumentException($"Unknown unit '{unitPart}'. Valid units are: {string.Join(", ", units.Keys)}");
        }
    }
}