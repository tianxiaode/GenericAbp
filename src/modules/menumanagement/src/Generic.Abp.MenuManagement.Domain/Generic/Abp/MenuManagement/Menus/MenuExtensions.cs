using System.Collections.Generic;
using Volo.Abp.Data;

namespace Generic.Abp.MenuManagement.Menus;

public static class MenuExtensions
{
    private const string PermissionsPropertyName = "permissions";

    public static void SetPermissions(this Menu entity, IEnumerable<string> permissions)
    {
        entity.SetProperty(PermissionsPropertyName, System.Text.Json.JsonSerializer.Serialize(permissions));
    }

    public static List<string> GetPermissions(this Menu entity)
    {
        var str = entity.GetProperty<string>(PermissionsPropertyName);
        return string.IsNullOrEmpty(str)
            ? new List<string>()
            : System.Text.Json.JsonSerializer.Deserialize<List<string>>(str) ?? new List<string>();
    }
}