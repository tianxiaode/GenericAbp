namespace Generic.Abp.Account
{
    public enum SendVerificationCodeResultType : byte
    {
        Success = 1,

        MaxCount = 2,

        Error = 3,

        InvalidEmail = 4
    }
}
