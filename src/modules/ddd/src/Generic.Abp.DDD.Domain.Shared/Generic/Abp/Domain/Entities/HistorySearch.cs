using System;

namespace Generic.Abp.Domain.Entities
{
    public class HistorySearch
    {
        public Guid ProductId { get; set; }

        public DateTime ModificationTime { get; set; }
    }
}
