using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Attributes
{
    /// <summary>
    /// 请求频率限制特性。 标记该特性的接口，会对用户的访问频率做出限制
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RateLimitAttribute : Attribute
    {
        public RateLimitAttribute(int requestIntervalSeconds = 5)
        {
            RequestIntervalSeconds = requestIntervalSeconds;
        }

        /// <summary>
        /// 请求频率间隔时间，默认为2秒。不建议超过5秒
        /// </summary>
        public int RequestIntervalSeconds { get; set; } = 2;
    }
}
