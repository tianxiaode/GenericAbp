using Generic.Abp.Extensions.Entities.Trees;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    public static class MenuManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureMenuManagement(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(MenuManagementDbProperties.DbTablePrefix + "Questions", MenuManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */

            var dbType = builder.GetDatabaseProvider();

            builder.Entity<Menu>(b =>
            {
                b.ToTable(MenuManagementDbProperties.DbTablePrefix + "Menus", MenuManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Name).IsRequired().HasMaxLength(TreeConsts.NameMaxLength);
                b.Property(m => m.Code).IsRequired().HasMaxLength(TreeConsts.GetCodeLength(TreeConsts.MaxDepth));
                b.Property(m => m.Icon).HasMaxLength(MenuConsts.IconMaxLength);
                b.Property(m => m.Router).HasMaxLength(MenuConsts.RouterMaxLength);
                b.Property(m => m.IsEnabled).IsRequired().HasDefaultValue(true);
                b.Property(m => m.Order).IsRequired().HasDefaultValue(1);
                b.Property(m => m.IsStatic).IsRequired().HasDefaultValue(false);
                b.Property(m => m.EntityVersion).IsRequired().HasDefaultValue(0);


                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.Name).UseCollation("gbk_chinese_ci");
                    b.Property(m => m.Code).UseCollation("ascii_general_ci");
                    b.Property(m => m.Icon).UseCollation("ascii_general_ci");
                    b.Property(m => m.Router).UseCollation("ascii_general_ci");
                }

                //Relations
                b.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId)
                    .IsRequired(false);

                //Indexes
                b.HasIndex(m => m.Code);
                b.HasIndex(m => m.Name);
                b.HasIndex(m => m.ParentId);
                b.HasIndex(m => m.Code);

                b.ApplyObjectExtensionMappings();
            });
        }
    }
}