using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Generic.Abp.Extensions.Extensions
{
    public static class StringExtensions
    {
        private const string RandomStringChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

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

            return str.First().ToString().ToUpperInvariant() + str[1..];
        }

        public static bool IsAscii(this string str)
        {
            return Encoding.UTF8.GetByteCount(str) == str.Length;
        }

        /// <summary>
        /// 随机生成字符串
        /// </summary>
        /// <param name="length">随机字符串长度</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            // Convert the random bytes to indices into the character array and build the string.
            return new string(bytes.Select(b => RandomStringChars[b % RandomStringChars.Length]).ToArray());
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

            var numericPart = double.Parse(str[..index]);

            if (index < str.Length)
            {
                unitPart = str[index..];
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

        public static T Parse<T>(this string? stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return default!;
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)stringValue!;
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)bool.Parse(stringValue)!;
                }

                if (typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(stringValue)!;
                }

                if (typeof(T) == typeof(long))
                {
                    return (T)(object)long.Parse(stringValue)!;
                }

                if (typeof(T) == typeof(double))
                {
                    return (T)(object)double.Parse(stringValue)!;
                }

                if (typeof(T) == typeof(decimal))
                {
                    return (T)(object)decimal.Parse(stringValue)!;
                }

                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)DateTime.Parse(stringValue)!;
                }

                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), stringValue);
                }

                return (T)Convert.ChangeType(stringValue, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse '{stringValue}' as {typeof(T).Name}.", ex);
            }
        }
    }
}