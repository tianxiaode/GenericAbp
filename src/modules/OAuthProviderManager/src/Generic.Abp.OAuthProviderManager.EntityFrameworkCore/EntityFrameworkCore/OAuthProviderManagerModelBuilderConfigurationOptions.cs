using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.OAuthProviderManager.EntityFrameworkCore
{
    public class OAuthProviderManagerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OAuthProviderManagerModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}