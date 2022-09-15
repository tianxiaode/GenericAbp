namespace Generic.Abp.Account
{
    public class VerificationCodeCacheItem
    {
        public int Count { get; set; }

        public string Code { get; set; }

        public VerificationCodeCacheItem(int count, string code)
        {
            Count = count;
            Code = code;
        }
    }
}
