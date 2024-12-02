using System;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultRetentionPolicyUpdateJobArgs(Guid? tenantId, string name, int? retentionPeriod, int batchSize)
    : JobBaseArgs(tenantId, name)
{
    public int? RetentionPeriod { get; set; } = retentionPeriod;
    public int BatchSize { get; set; } = batchSize;
}