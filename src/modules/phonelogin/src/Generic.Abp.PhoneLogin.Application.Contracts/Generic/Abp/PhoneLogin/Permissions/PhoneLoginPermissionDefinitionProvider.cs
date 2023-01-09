using Generic.Abp.PhoneLogin.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.PhoneLogin.Permissions
{
    public class PhoneLoginPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PhoneLoginPermissions.GroupName, L("Permission:PhoneLogin"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PhoneLoginResource>(name);
        }
    }
}