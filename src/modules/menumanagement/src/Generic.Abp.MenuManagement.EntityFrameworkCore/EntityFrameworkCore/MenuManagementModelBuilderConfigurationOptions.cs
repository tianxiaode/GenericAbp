using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    public class MenuManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public MenuManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}