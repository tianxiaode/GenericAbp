﻿namespace Generic.Abp.BusinessException
{
    public static class BusinessExceptionErrorCodes
    {
        //Add your business exception error codes here...
        public const string DuplicateWarning = "Generic.Abp.BusinessException:000001";

        public const string DuplicateWarningParamName = "Name";

        public const string DuplicateWarningParamValue = "Value";

        public const string HasChildrenCanNotDeleted = "Generic.Abp.BusinessException:000002";

        public const string HasChildrenCanNotDeletedParamName = "Name";

        public const string HasChildrenCanNotDeletedParamValue = "Value";

        public const string ValueExceedsFieldLength = "Generic.Abp.BusinessException:000003";

        public const string ValueExceedsFieldLengthParamValue = "Value";

        public const string ValueExceedsFieldLengthParamLength = "Length";

        public const string NoPermissionAccess = "Generic.Abp.BusinessException:000004" ;

        public const string NoPermissionEdit = "Generic.Abp.BusinessException:000005" ;

        public const string NoPermissionDelete = "Generic.Abp.BusinessException:000006" ;

        public const string NoPermissionParamName =  "Name";

        public const string NoPermissionParamValue =  "Value";

        public const string UnknownOrganization = "Generic.Abp.BusinessException:000007" ;

    }
}
