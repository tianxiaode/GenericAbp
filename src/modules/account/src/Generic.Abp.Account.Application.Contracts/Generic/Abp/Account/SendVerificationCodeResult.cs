namespace Generic.Abp.Account
{
    public class SendVerificationCodeResult
    {
        public SendVerificationCodeResult(SendVerificationCodeResultType result)
        {
            Result = result;
        }

        public SendVerificationCodeResultType Result { get; }

        public string Description => Result.ToString();

    }
}
