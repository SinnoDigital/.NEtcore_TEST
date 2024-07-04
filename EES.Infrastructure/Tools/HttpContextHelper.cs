using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Tools
{
    public class HttpContextHelper
    {
        /// <summary>
        /// 获取JwtToken,不包含Bearer
        /// </summary>
        /// <returns></returns>
        public static string GetJwtTokenWithoutBearer(HttpContext context)
        {
            var _token = GetToken(context);
            
            return string.IsNullOrWhiteSpace(_token) || _token.Length <= 7 ? string.Empty : _token[7..];
        }


        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private static string GetToken(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                return context.Request.Headers["Authorization"];
            }
            return string.Empty;
        }
    }
}
