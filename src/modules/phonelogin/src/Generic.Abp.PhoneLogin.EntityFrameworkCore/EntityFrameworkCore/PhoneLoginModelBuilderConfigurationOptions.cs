using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.PhoneLogin.EntityFrameworkCore
{
    public class PhoneLoginModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PhoneLoginModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}