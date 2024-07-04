using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Log;
using EES.Infrastructure.Service;
using EES.Infrastructure.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        private readonly ILogDispatchProvider _logDispatchProvider;

        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, ILogDispatchProvider logDispatchProvider, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _logDispatchProvider = logDispatchProvider;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex, "程序发生了异常");

                var log = GetExceptionLog(ex, context.TraceIdentifier);

                if (log != null)
                {
                    await _logDispatchProvider.PublishAsync(log);
                }

                ApiResponseBase response;

                if (_environment.IsDevelopment())
                {
                    response = ApiResponseBase.Fail(999, $"服务器异常，错误信息:{ex.Message}.Trace Id:{traceId}");
                }
                else
                {
                    response = ApiResponseBase.Fail(BusinessError.服务器异常);
                }
                // 设置响应的内容类型和状态码
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;

                // 将JSON文本写入响应
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }


        public static ExceptionLog GetExceptionLog(Exception ex, string traceIdentifier)
        {
            if (ex is null)
            {
                return null;
            }

            return new ExceptionLog
            {
                ExceptionType = ex.GetGenericTypeName(),
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Flag = Guid.NewGuid(),
                OccurredTime = DateTime.Now,
                Text = ExceptionHelper.GetCompleteErrorMessage(ex),
                TraceId = string.IsNullOrEmpty(traceIdentifier) ? HttpAccessor.TraceIdentifier : traceIdentifier
            };
        }
    }
}
