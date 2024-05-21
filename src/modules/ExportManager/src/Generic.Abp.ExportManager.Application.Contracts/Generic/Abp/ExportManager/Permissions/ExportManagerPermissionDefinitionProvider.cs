using Generic.Abp.ExportManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.ExportManager.Permissions
{
    public class ExportManagerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(ExportManagerPermissions.GroupName, L("Permission:ExportManager"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ExportManagerResource>(name);
        }
    }
}