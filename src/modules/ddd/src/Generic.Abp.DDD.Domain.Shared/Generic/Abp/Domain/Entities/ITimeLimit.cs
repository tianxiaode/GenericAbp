using System;

namespace Generic.Abp.Domain.Entities
{
    /// <summary>
    /// 期限接口
    /// </summary>
    public interface ITimeLimit
    {

        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime StartTime { get;  }

        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime EndTime { get; }
    }
}