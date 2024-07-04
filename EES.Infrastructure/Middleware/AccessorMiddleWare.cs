using EES.Infrastructure.Service;
using FreeRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Jwt;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using EES.Infrastructure.Cache;
using EES.Infrastructure.Auth;
using EES.Infrastructure.Tools;

namespace EES.Infrastructure.Middleware
{
    /// <summary>
    /// 缓存当前请求的用户信息中间件
    /// </summary>
    public class AccessorMiddleWare
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<AccessorMiddleWare> _logger;

        private readonly RedisClient _redisClient;

        // private readonly ITokenService _tokenService;

        public AccessorMiddleWare(RequestDelegate next, ILogger<AccessorMiddleWare> logger, RedisClient redisClient)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient;
            // _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpAccessor.SetTraceIdentifier(context.TraceIdentifier);

            bool needRefreshToken = false;

            Accessor user = new();

            var claims = context.User.Claims;

            var userId = claims.FirstOrDefault(item => item.Type == "id")?.Value;

            var platform = claims.FirstOrDefault(item => item.Type == "platform")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                HttpAccessor.SetAccessor(Accessor.Anonymous());
            }
            else
            {
                user.Id = long.Parse(userId);

                string authKey = CacheKeyProvider.GetAuthCacheKey(userId);

                string tokenKey = CacheKeyProvider.GetTokenCacheKey(platform!, userId);

                try
                {
                    var token = HttpContextHelper.GetJwtTokenWithoutBearer(context);

                    if (JwtTokenProvider.TryGetExpirationTimestamp(token, out int? exp))
                    {
                        var value = int.Parse(_redisClient.Get(tokenKey));
                        if (exp.HasValue && value != exp.Value)
                        {
                            await Response(context, BusinessError.用户未登录或登录状态已过期, 401);
                            return;
                        }
                        else
                        {
                            var tokenTime = exp.Value.ToDateTime();

                            if ((tokenTime - DateTime.Now).TotalMinutes < 15) //token的过期时间少于15分钟;
                            {
                                needRefreshToken = true;
                            }
                        }

                    }
                    else
                    {
                        //未成功解析，则视为非法的token
                        await Response(context, BusinessError.您无权进行此操作, 403);
                        return;
                    }

                    if (_redisClient.Exists(authKey))
                    {
                        var value = _redisClient.Get(authKey);

                        var auth = JsonConvert.DeserializeObject<AuthInfo>(value);

                        if (auth != null)
                        {
                            user.Name = auth.User.Name;
                            user.AuthData = auth.AuthData;
                            user.AuthFunctions = auth.AuthFunctions;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "读取用户的权限资源的缓存信息异常,Key：{key}", authKey);

                    await Response(context, BusinessError.身份鉴权异常);
                    return;
                }

                HttpAccessor.SetAccessor(user);
            }


            if (needRefreshToken)
            {
                var newToken = JwtTokenProvider.GenerateToken(userId!, platform!);

                if (JwtTokenProvider.TryGetExpirationTimestamp(newToken, out int? exp))
                {
                    var key = CacheKeyProvider.GetTokenCacheKey(platform!.ToLower(), user.Id.ToString());

                    _redisClient.Set(key, exp, CacheKeyProvider.TOKEN_EXPIRE_MINUTE * 60);
                }

                context.Response.Headers.Add("Authorization", "Bearer " + newToken);
            }

            await _next(context);          
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="error"></param>
        /// <param name="httpCode"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static async Task Response(HttpContext context, BusinessError error, int httpCode = 200, string contentType = "application/json")
        {

            var message = GetMessage(error);

            context.Response.StatusCode = httpCode;
            context.Response.ContentType = contentType;
            await context.Response.WriteAsync(message);
        }

        private static string GetMessage(BusinessError error)
        {
            return JsonConvert.SerializeObject(ApiResponseBase.Fail(error));
        }
    }
}
