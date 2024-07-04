using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Attributes
{
    /// <summary>
    /// 幂等性保证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class IdempotentAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag">幂等性接口标识符</param>
        /// <param name="intervalSeconds">幂等性间隔时间，默认为5秒。不建议超过10秒</param>
        public IdempotentAttribute(string flag = "flag", int intervalSeconds = 5)
        {
            Flag = flag;
            IntervalSeconds = intervalSeconds;
        }

        /// <summary>
        /// 接口的幂等标记(不能重复)
        /// </summary>
        public string Flag { get; set; } = "flag";

        /// <summary>
        /// 幂等性间隔时间，默认为5秒。不建议超过10秒
        /// </summary>
        public int IntervalSeconds { get; set; } = 5;
    }
}
