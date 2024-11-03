namespace Generic.Abp.Extensions
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

        public const string NoPermissionAccess = "Generic.Abp.BusinessException:000004";

        public const string NoPermissionEdit = "Generic.Abp.BusinessException:000005";

        public const string NoPermissionDelete = "Generic.Abp.BusinessException:000006";

        public const string NoPermissionParamName = "Name";

        public const string NoPermissionParamValue = "Value";

        public const string UnknownOrganization = "Generic.Abp.BusinessException:000007";

        public const string DateRangeByMonth = "Generic.Abp.BusinessException:000008";

        public const string DateRangeByMonthValue = "Value";

        public const string DateRangeByDays = "Generic.Abp.BusinessException:000009";

        public const string DateRangeByDaysValue = "Value";

        public const string EntityNotFound = "Generic.Abp.BusinessException:000010";

        public const string EntityNotFoundParamName = "Name";

        public const string EntityNotFoundParamValue = "Value";

        public const string EntityNotBeDeleted = "Generic.Abp.BusinessException:000011";

        public const string EntityNotBeDeletedParamName = "Name";

        public const string EntityNotBeDeletedParamValue = "Value";

        public const string UnknownParent = "Generic.Abp.BusinessException:000012";

        public const string StaticEntityCanNotBeUpdatedOrDeleted = "Generic.Abp.BusinessException:000013";

        public const string StaticEntityCanNotBeUpdatedOrDeletedParamName = "Name";

        public const string StaticEntityCanNotBeUpdatedOrDeletedParamValue = "Value";
    }
}