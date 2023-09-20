using System;
using Generic.Abp.Domain.Entities;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    public static class MenuManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureMenuManagement(
            this ModelBuilder builder,
            Action<MenuManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MenuManagementModelBuilderConfigurationOptions(
                MenuManagementDbProperties.DbTablePrefix,
                MenuManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);

                b.ConfigureByConvention();

                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */

            builder.Entity<Menu>(b =>
            {
                b.ToTable(options.TablePrefix + "Menus", options.Schema);

                b.ConfigureByConvention();

                //Properties
                b.Property(q => q.DisplayName).IsRequired().HasMaxLength(TreeConsts.DisplayNameMaxLength)
                    .UseCollation("gbk_chinese_ci");
                b.Property(m => m.Code).IsRequired().HasMaxLength(TreeConsts.CodeMaxLength)
                    .UseCollation("ascii_general_ci");
                b.Property(q => q.Icon).HasMaxLength(MenuConsts.IconMaxLength).UseCollation("gbk_chinese_ci");
                b.Property(q => q.Router).HasMaxLength(MenuConsts.RouterMaxLength).UseCollation("gbk_chinese_ci");
                b.Property(q => q.GroupName).HasMaxLength(MenuConsts.GroupNameMaxLength).UseCollation("gbk_chinese_ci");


                //Relations
                b.HasOne<Menu>(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId)
                    .IsRequired(false);

                //Indexes
                b.HasIndex(m => m.Code);
                b.HasIndex(m => m.DisplayName);
                b.HasIndex(m => m.ParentId);
                b.HasIndex(m => m.GroupName);
                b.HasIndex(m => m.Order);
            });
        }
    }
}