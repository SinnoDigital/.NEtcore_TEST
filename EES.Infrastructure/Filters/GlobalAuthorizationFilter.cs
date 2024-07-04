using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using FreeRedis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EES.Infrastructure.Filters
{
    /// <summary>
    /// 全局的权限过滤
    /// </summary>
    public class GlobalAuthorizationFilter : IAuthorizationFilter
    {
        private readonly RedisClient _redisClient;

        private ILogger<GlobalAuthorizationFilter> _logger;

        public GlobalAuthorizationFilter(RedisClient redisClient, ILogger<GlobalAuthorizationFilter> logger)
        {
            _redisClient = redisClient;
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 获取Action上的特性
            var authorizationAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizationRequiredAttribute>().FirstOrDefault();

            if (authorizationAttribute != null)
            {
                string requiredPermission = authorizationAttribute.Permission.ToLower().Trim();

                if (!HttpAccessor.Accessor.AuthFunctions.Any(t => t.Identifier == requiredPermission))
                {
                    _logger.LogInformation("权限校验失败! Action:{actionName}，UserId：{userId}", context.ActionDescriptor.DisplayName, HttpAccessor.Accessor.Id);

                    var response = ApiResponseBase.Fail(BusinessError.您无权进行此操作);

                    context.Result = new JsonResult(response)//JsonConvert.SerializeObject()
                    {
                        StatusCode = 200
                    };
                }

            }
        }     
    }
}
