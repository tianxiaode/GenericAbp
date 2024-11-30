using System.Collections.Generic;
using System.Linq;

namespace Generic.Abp.FileManagement.Resources;

public static class ProviderNames
{
    public const string UserProviderName = "U";
    public const string RoleProviderName = "R";
    public const string AuthorizationUserProviderName = "A";
    public const string EveryoneProviderName = "E";

    // 获取所有定义的值
    public static IReadOnlyList<string> GetAllValues()
    {
        return new List<string>
        {
            UserProviderName,
            RoleProviderName,
            AuthorizationUserProviderName,
            EveryoneProviderName
        };
    }

    // 检查值是否合法
    public static bool IsValid(string providerName)
    {
        return GetAllValues().Contains(providerName);
    }
}