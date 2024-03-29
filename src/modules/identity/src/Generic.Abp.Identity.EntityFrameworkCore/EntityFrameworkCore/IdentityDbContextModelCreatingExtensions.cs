﻿using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.EntityFrameworkCore
{
    public static class IdentityDbContextModelCreatingExtensions
    {
        public static void ConfigureIdentity(
            this ModelBuilder builder,
            Action<IdentityModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityModelBuilderConfigurationOptions(
                AbpIdentityDbProperties.DbTablePrefix,
                AbpIdentityDbProperties.DbSchema
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
        }
    }
}