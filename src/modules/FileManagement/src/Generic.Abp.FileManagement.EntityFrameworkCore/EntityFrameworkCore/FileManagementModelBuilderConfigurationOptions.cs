using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    public class FileManagementModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix = "",
        string? schema = null)
        : AbpModelBuilderConfigurationOptions(tablePrefix,
            schema);
}