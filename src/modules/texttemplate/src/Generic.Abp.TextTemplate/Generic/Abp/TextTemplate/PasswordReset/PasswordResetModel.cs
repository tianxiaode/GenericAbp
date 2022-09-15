namespace Generic.Abp.TextTemplate.PasswordReset
{
    public class PasswordResetModel
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }


        public PasswordResetModel(string companyName, string userName, string verificationCode)
        {
            Company = companyName;
            Name = userName;
            Code = verificationCode;
        }
    }
}
