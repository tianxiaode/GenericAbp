using System;
using System.Linq;

namespace Generic.Abp.Helper.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToQueryString(this object obj)
        {

            return string.Join("&", obj.GetType()
                .GetProperties()
                .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj).ToString())}"));
        }
    }
}