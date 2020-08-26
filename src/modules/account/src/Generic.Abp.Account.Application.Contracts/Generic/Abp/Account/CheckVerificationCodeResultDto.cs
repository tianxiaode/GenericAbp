namespace Generic.Abp.Account
{
    public class CheckVerificationCodeResultDto
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }

        public CheckVerificationCodeResultDto(string emailAddress, string token)
        {
            EmailAddress = emailAddress;
            Token = token;
        }
    }
}
