using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Cache
{
    public class LuaScript
    {
        /// <summary>
        /// 若key存在，则获取值,否则添加key，并写入值，然后返回"success"字符串
        /// </summary>
        public static readonly string GetOrSet = @"
            local key = KEYS[1]
            local value = redis.call('GET', key)
            
            if value then
                return value
            else
                local newValue = ARGV[1]
                local expiration = tonumber(ARGV[2])
            
                redis.call('SET', key, newValue)
            
                if expiration > 0 then
                    redis.call('EXPIRE', key, expiration)
                end
            
                return 'success'
            end
        ";
    }
}
