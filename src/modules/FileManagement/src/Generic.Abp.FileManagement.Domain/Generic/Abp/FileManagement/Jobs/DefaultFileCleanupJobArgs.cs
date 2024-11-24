using System;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultFileCleanupJobArgs(Guid? tenantId, string name, int batchSize) : JobBaseArgs(tenantId, name)
{
    public int BatchSize { get; set; } = batchSize;
}