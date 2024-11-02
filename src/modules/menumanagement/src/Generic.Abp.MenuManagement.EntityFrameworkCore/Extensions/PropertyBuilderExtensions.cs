using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Generic.Abp.MenuManagement.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> UseCollationIfSupported<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder,
        string collation)
    {
        // 获取 PropertyBuilder 类型
        var propertyBuilderType = propertyBuilder.GetType();

        // 获取 UseCollation 方法
        var useCollationMethod = propertyBuilderType.GetMethod("UseCollation", [typeof(string)]);

        if (useCollationMethod != null)
        {
            // 调用 UseCollation 方法
            useCollationMethod.Invoke(propertyBuilder, [collation]);
        }
        else
        {
            // 处理不支持 UseCollation 的情况
            Console.WriteLine("UseCollation method is not supported by the current database provider.");
        }

        return propertyBuilder;
    }
}