﻿using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.MyProjectName.EntityFrameworkCore
{
    [ConnectionStringName(MyProjectNameDbProperties.ConnectionStringName)]
    public interface IMyProjectNameDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}