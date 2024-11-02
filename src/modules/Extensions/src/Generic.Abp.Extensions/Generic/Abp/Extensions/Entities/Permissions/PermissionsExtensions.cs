using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.Data;

namespace Generic.Abp.Extensions.Entities.Permissions;

public static class PermissionsExtensions
{
    private const string PermissionsPropertyName = "Permissions";

    public static void SetPermissions<TEntity>(this TEntity entity, ICollection<string> permissions)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(PermissionsPropertyName, permissions);
    }

    public static List<string> GetPermissions<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        var permissions = entity.GetProperty(PermissionsPropertyName);
        if (permissions == null)
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<string>>(((JsonElement)permissions).GetRawText()) ?? [];
    }
}