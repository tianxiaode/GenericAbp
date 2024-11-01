using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.Extensions.Validates;

public static class Validator
{
    // 扩展方法，用于异步验证对象的最大长度
    public static Task<ValidateResult> MaxLengthAsync<T>(this T obj, int maxLength)
    {
        var result = new ValidateResult();
        if (obj == null)
        {
            return Task.FromResult(result);
        }

        switch (obj)
        {
            // 验证字典中所有值的长度是否超过最大限制
            case Dictionary<string, object> dictionary:
                //找出字典中长度最大的项
                var maxLengthItem = dictionary.OrderByDescending(item => item.Value.ToString()!.Length)
                    .FirstOrDefault();
                result.Value = maxLengthItem.Value.ToString()!;
                result.Success = result.Value.Length <= maxLength;
                return Task.FromResult(result);
            // 验证集合中任何字符串项的长度是否超过最大限制
            case ICollection<string> collection:
                var maxLengthString = collection.MaxBy(item => item.Length);
                result.Value = maxLengthString;
                result.Success = maxLengthString?.Length <= maxLength;
                return Task.FromResult(result);
            // 验证字符串的长度是否超过最大限制
            case string str:
                result.Value = str;
                result.Success = str.Length <= maxLength;
                return Task.FromResult(result);
            default:
                // 对于其他类型，返回真
                return Task.FromResult(result);
        }
    }
}