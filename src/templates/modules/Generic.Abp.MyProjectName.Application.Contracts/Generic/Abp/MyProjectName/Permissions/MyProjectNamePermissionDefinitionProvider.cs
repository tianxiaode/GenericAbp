﻿using Generic.Abp.MyProjectName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Generic.Abp.MyProjectName.Permissions
{
    public class MyProjectNamePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(MyProjectNamePermissions.GroupName, L("Permission:MyProjectName"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MyProjectNameResource>(name);
        }
    }
}