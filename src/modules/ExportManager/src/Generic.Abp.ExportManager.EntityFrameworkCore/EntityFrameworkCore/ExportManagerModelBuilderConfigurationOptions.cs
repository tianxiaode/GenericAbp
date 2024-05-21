using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.ExportManager.EntityFrameworkCore
{
    public class ExportManagerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ExportManagerModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}