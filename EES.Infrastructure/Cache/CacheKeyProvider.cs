using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Cache
{
    public class CacheKeyProvider
    {
        private static readonly string _tokenCacheKeyFormat = "token:{platform}:{userId}"; //token 存储的key

        private static readonly string _authCacheKeyFormat = "auth:{userId}"; //用户权限信息缓存key

        public static readonly int TOKEN_EXPIRE_MINUTE = 60 * 18; //默认的Token有效期

        private static readonly string _requestRateLimitKey = "request:{platform}:{userId}";

        private static readonly string _idempotentKey = "idempotent:{userId}:{flag}"; //幂等性保证的key

        /// <summary>
        /// 获取token的缓存key
        /// </summary>
        /// <param name="platform">平台</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public static string GetTokenCacheKey(string platform, string userId)
        {
            return _tokenCacheKeyFormat.Replace("{platform}", platform).Replace("{userId}", userId);
        }

        /// <summary>
        /// 获取请求时间缓存
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetRequestRateLimitKey(string platform, string userId)
        {
            return _requestRateLimitKey.Replace("{platform}", platform).Replace("{userId}", userId);
        }

        /// <summary>
        /// 获取用户权限数据的缓存key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetAuthCacheKey(string userId)
        {
            return _authCacheKeyFormat.Replace("{userId}", userId);
        }

        /// <summary>
        /// 获取幂等性key
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetIdempotentKey(string userId, string flag)
        {
            return _idempotentKey.Replace("{userId}", userId).Replace("{flag}", flag);
        }

        public static string GetIdempotentKeyPrefix(string userId)
        {
            var list = _idempotentKey.Split(':').ToList();

            list.RemoveAt(list.Count - 1);

            return string.Join(":", list).Replace("{userId}", userId);
        }
    }
}
