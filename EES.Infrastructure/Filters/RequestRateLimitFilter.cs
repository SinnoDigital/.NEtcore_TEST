using EES.Infrastructure.Attributes;
using EES.Infrastructure.Cache;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using FreeRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Filters
{
    /// <summary>
    /// 请求频率限制过滤器
    /// </summary>
    public class RequestRateLimitFilter : IAsyncActionFilter
    {
        private readonly RedisClient _redisClient;

        private readonly ILogger<RequestRateLimitFilter> _logger;

        public RequestRateLimitFilter(RedisClient redisClient, ILogger<RequestRateLimitFilter> logger)
        {
            _redisClient = redisClient;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var attr = context.ActionDescriptor.EndpointMetadata.OfType<RateLimitAttribute>().FirstOrDefault();

            var method = context.HttpContext.Request.Method.ToUpper();

            /*
             本想记录匿名访问者的IP，但是实际应用当中，客户内网环境复杂，经常出现多个用户是同一个IP的情况
             */
            if (attr != null && HttpAccessor.Accessor.Id != 0 && method != "OPTIONS")
            {
                             
                TryGetUser(context.HttpContext, out string userId, out string platform);

                string key = CacheKeyProvider.GetRequestRateLimitKey(platform, userId);

                var now = DateTime.Now;

                if (_redisClient.Exists(key))
                {
                    var lastRequestTime = DateTime.Parse(_redisClient.Get(key));

                    if (now - lastRequestTime < TimeSpan.FromSeconds(attr.RequestIntervalSeconds))
                    {
                        // 如果两次请求的时间间隔小于2秒，返回429 Too Many Requests响应
                        //context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        //context.Response.Headers.Add("Retry-After", _requestInterval.TotalSeconds.ToString());
                        //await context.Response.WriteAsync("操作太频繁了，请稍后重试");


                        var response = ApiResponseBase.Fail(BusinessError.请求的太频繁了);

                        context.Result = new JsonResult(response)
                        {
                            StatusCode = 200, // 设置响应的内容类型和状态码

                            ContentType = "application/json"
                        };
                        return;
                    }
                }
                _redisClient.Set(key, now.ToString("yyyy-MM-dd HH:mm:ss"));//更新记录
            }

            await next();
        }

        public static string GetIp(HttpContext context)
        {

            string requestIp = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(requestIp) || requestIp == "127.0.0.1")
            {
                //Nginx代理转发之后，可能拿不到真实请求IP，需要在Nginx里面配置，将真实IP放在HttpRequest的Header里面，并且约定好请求头

                if (context.Request.Headers.ContainsKey("x-real-ip"))
                {
                    requestIp = context.Request.Headers["x-real-ip"];
                }
            }

            if (requestIp == "::1")
            {
                requestIp = "127.0.0.1";
            }

            return requestIp;

        }

        public static bool TryGetUser(HttpContext context, out string? userId, out string? platform)
        {

            var claims = context.User.Claims;

            userId = claims.FirstOrDefault(item => item.Type == "id")?.Value;

            platform = claims.FirstOrDefault(item => item.Type == "platform")?.Value;

            return true;


        }
    }
}
