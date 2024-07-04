using EES.Infrastructure.Attributes;
using EES.Infrastructure.Cache;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Infrastructure.Tools;
using FreeRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Filters
{
    /// <summary>
    /// 幂等性保证过滤器
    /// </summary>
    public class IdempotentAsyncFilter : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        private readonly RedisClient _redisClient;

        private readonly ILogger<IdempotentAsyncFilter> _logger;

        private readonly int _defaultExpTime = 30;//预设的过期时间，单位是秒


        public IdempotentAsyncFilter(RedisClient redisClient, ILogger<IdempotentAsyncFilter> logger)
        {
            _redisClient = redisClient;
            _logger = logger;
        }


        private IDictionary<string, object?> _actionArguments; //请求数据

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var attr = context.ActionDescriptor.EndpointMetadata.OfType<IdempotentAttribute>().FirstOrDefault();

            if (attr != null && HttpAccessor.Accessor.Id != 0) //确保万一，匿名访问的接口不做幂等性保证        
            {
                //_logger.LogInformation("OnActionExecutionAsync  userId={userid} ,falg={flag}", HttpAccessor.Accessor.Id, attr.Flag);

                DeepCopyActionArguments(context); // 将请求参数进行备份

                 var key = GetIdempotentKey(HttpAccessor.Accessor.Id.ToString(), attr.Flag);

                var value = JsonConvert.SerializeObject(ApiResponseBase.Instance(BusinessError.请求正在处理中));

                //执行lua脚本
                var res = _redisClient.Eval(LuaScript.GetOrSet, new string[1] { key }, new object[] { value, _defaultExpTime }).ToString();

                if (!(res == "success"))
                {
                    var message = JsonConvert.DeserializeObject<ApiResponseBase>(res);
                    _logger.LogInformation("幂等性风控触发，请求被拦截！请求相应结果：{message}", res);
                    context.Result = new JsonResult(message)
                    {
                        StatusCode = 200, // 设置响应的内容类型和状态码

                        ContentType = "application/json"
                    };
                    return;
                }
            }

            await next();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next();

            var attr = context.ActionDescriptor.EndpointMetadata.OfType<IdempotentAttribute>().FirstOrDefault();

            if (attr != null && HttpAccessor.Accessor.Id != 0) //确保万一，匿名访问的接口不做幂等性保证            
            {
                //_logger.LogInformation("OnResultExecutionAsync  userId={userid} ,falg={flag}", HttpAccessor.Accessor.Id, attr.Flag);

                var key = GetIdempotentKey(HttpAccessor.Accessor.Id.ToString(), attr.Flag);

                string str = string.Empty;

                try
                {
                    //获取到业务执行完毕之后返回的结果

                    if (context.Result is JsonResult jsonResult)
                    {
                        str = JsonConvert.SerializeObject(jsonResult.Value);
                    }
                    else if (context.Result is ObjectResult objectResult)
                    {
                        var obj = JObject.FromObject(objectResult.Value!);

                        str = JsonConvert.SerializeObject(obj);
                    }
                    else
                    {
                        _logger.LogInformation("都没匹配上，Type:{type}", context.Result.GetType());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "解析返回结果出现异常");
                }
                /*
                    即便是请求失败了，也将结果缓存起来。因为极短的时间内，如果第一次请求失败了，那么后续的请求基本上也都是失败的
                 */
                _redisClient.Set(key, str, attr.IntervalSeconds);
            }

            if (_actionArguments != null)
            {
                _actionArguments = null;
            }
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var attr = context.ActionDescriptor.EndpointMetadata.OfType<IdempotentAttribute>().FirstOrDefault();

            if (attr != null && HttpAccessor.Accessor.Id != 0)
            {
                var key = GetIdempotentKey(HttpAccessor.Accessor.Id.ToString(), attr.Flag);

                var message = GetMessage(BusinessError.服务器异常);

                _redisClient.Set(key, message, attr.IntervalSeconds); //发生异常了也要把结果缓存起来
            }

            if (_actionArguments != null)
            {
                _actionArguments = null;
            }

            return Task.CompletedTask;
        }

        private string GetKeyFlag(string attrFlag)
        {
            //_logger.LogInformation("actionArguments={actionArguments}", JsonConvert.SerializeObject(_actionArguments));

            return SignHelper.GetGetIdempotentKeyFlag(_actionArguments, attrFlag);
        }

        private string GetIdempotentKey(string userId, string attrFlag)
        {

            var keyFlag = GetKeyFlag(attrFlag);

            //_logger.LogInformation("keyFlag={flag}", keyFlag);

            return CacheKeyProvider.GetIdempotentKey(userId, keyFlag);
        }

        private static string GetMessage(BusinessError error)
        {
            return JsonConvert.SerializeObject(ApiResponseBase.Fail(error));
        }


        /// <summary>
        ///对请求参数进行深拷贝
        /// </summary>
        ///<remarks>部分请求参数的数据结构体较为复杂，且具体执行代码会对该请求参数进行修改，为了保证验证的准确性，需要将原始请求值进行深拷贝备份</remarks>
        /// <param name="context"></param>
        private void DeepCopyActionArguments(ActionExecutingContext context)
        {
            if (context.ActionArguments == null || !context.ActionArguments.Any())
            {
                return;
            }
           
            var json = JsonConvert.SerializeObject(context.ActionArguments);

            _actionArguments = JsonConvert.DeserializeObject<IDictionary<string, object?>>(json)!;
        }     
    }
}

