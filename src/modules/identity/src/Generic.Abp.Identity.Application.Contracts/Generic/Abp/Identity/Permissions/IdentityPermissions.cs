﻿using Volo.Abp.Reflection;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Identity.Permissions
{
    public class IdentityPermissions
    {
        public const string GroupName = SettingManagementPermissions.GroupName;

        public const string IdentityManagement = GroupName + ".IdentityManagement";


        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
        }
    }
}