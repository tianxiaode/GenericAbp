namespace Generic.Abp.BusinessException
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
    }
}
