﻿using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Identity.Permissions
{
    public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.GetGroup(SettingManagementPermissions.GroupName);
            myGroup.AddPermission(IdentityPermissions.IdentityManagement, L("Permission:IdentityManagement"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IdentityResource>(name);
        }
    }
}