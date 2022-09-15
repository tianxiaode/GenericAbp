using System;

namespace Generic.Abp.Domain.Entities
{
    public static class HistoryKeyGenerator
    {
        public const int KeyMaxLength = 64;
        public static string Create(Guid id, DateTime time)
        {
            return $"{id}::{time:yyyy-MM-dd HH:mm:ss.ffffff}";
        }
    }
}
