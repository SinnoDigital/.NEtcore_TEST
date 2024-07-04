using EES.Infrastructure.Jwt;
using EES.Infrastructure.Log;
using EES.Infrastructure.Service;
using EES.Infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EES.Infrastructure.Middleware
{
    /// <summary>
    /// 日志中间件，记录所有的请求的 request 和 response
    /// </summary>
    public class LogMiddleWare
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<LogMiddleWare> _logger;

        private readonly Stopwatch _stopwatch;

        private readonly ILogDispatchProvider _logDispatchProvider;

        public LogMiddleWare(RequestDelegate next, ILogger<LogMiddleWare> logger, ILogDispatchProvider logDispatchProvider)
        {
            _next = next;
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
            _logDispatchProvider = logDispatchProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method.ToUpper();

            if (method == "OPTIONS")
            {
                await _next(context);
            }
            else
            {
                _stopwatch.Restart();

                context.Request.EnableBuffering(); // 这行不能漏了

                ApiLog log = await GetRequest(context);

                context.Request.Body.Position = 0; //读取完数据之后，进行复位，这行也不能漏

                _stopwatch.Restart();

              


                if (log.ApiRoute.StartsWith(@"/static"))//静态文件请求
                {
                    await _next(context);

                    log.Response = "静态文件，图片或模板类操作，数据过大，不做记录";
                }
                else
                {
                    var responseStream = context.Response.Body;
                    using var newStream = new MemoryStream();
                    context.Response.Body = newStream;//将Body换成新的流，空的
                    StreamReader? responseReader = null;

                    try
                    {
                        await _next(context);

                        newStream.Position = 0;
                        responseReader = new StreamReader(newStream);
                        log.Response = await responseReader.ReadToEndAsync();
                    }
                    finally
                    {
                        newStream.Position = 0;
                        await newStream.CopyToAsync(responseStream);
                        context.Response.Body = responseStream;

                        responseReader?.Dispose();
                    }
                }
              
                _stopwatch.Stop();

                log.TimeConsumption = _stopwatch.ElapsedMilliseconds;

                await _logDispatchProvider.PublishAsync(log); //发布日志

                _logger.LogInformation("Request:【UserId={userId}】【platform={platform}】【 Method={method}】【RequestUrl:{url}】【RequestHeader:{header}】【QueryStrings={query}】【Body={body}】==== Response:{response}==== TimeCost:{cost} Milliseconds", log.UserId, log.Platform, log.RequestType, log.RequestUrl, log.RequestHeader, log.RequestQuery, log.RequestBody, log.Response, log.TimeConsumption);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task<ApiLog> GetRequest(HttpContext context)
        {

            ApiLog log = new();

            GetRequestUser(context, ref log);

            log.TraceId = context.TraceIdentifier;

            log.RequestType = context.Request.Method.ToLower();

            log.ApiRoute = context.Request.Path;

            log.RequestQuery = context.Request.QueryString.ToString();

            log.RequestUrl = HttpUtility.UrlDecode(context.Request.GetDisplayUrl());

            log.RequestHeader = JsonConvert.SerializeObject(context.Request.Headers);

            if (!log.ApiRoute.ToLower().StartsWith(@"/api/system"))
            {
                log.RequestBody = await GetRequestBodyAsync(context.Request.BodyReader);
            }
            else
            {
                log.RequestBody = "文件，图片或模板类操作，数据过大，不做记录";
            }

            log.RequestTime = DateTime.Now;

            return log;
        }


        /// <summary>
        /// 获取当前请求用户
        /// </summary>
        /// <param name="context"></param>
        /// <param name="log"></param>
        private void GetRequestUser(HttpContext context, ref ApiLog log)
        {
            var token = HttpContextHelper.GetJwtTokenWithoutBearer(context);

            if (!string.IsNullOrWhiteSpace(token))
            {
                if (JwtTokenProvider.TryGetClaim(token, out string? platform, out long? userId))
                {
                    log.Platform = platform;
                    log.UserId = userId ?? 0;
                }
                else
                {
                    _logger.LogInformation("尝试从token中解析userId失败! token已过期或者token无效。 token:{token}", token);
                    log.UserId = 0;
                    log.Platform = string.Empty;
                }
            }
            else
            {
                log.UserId = 0;
                log.Platform = string.Empty;
            }
        }


        /// <summary>
        /// 读取request的Body数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static async Task<string> GetRequestBodyAsync(PipeReader reader)
        {
            StringBuilder builder = new();

            while (true)
            {
                ReadResult readResult = await reader.ReadAsync();

                var buffer = readResult.Buffer;

                var readOnlySequence = buffer.Slice(0);

                if (readResult.IsCompleted && buffer.Length > 0)
                {
                    Append(builder, in readOnlySequence);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (readResult.IsCompleted)
                {
                    break;
                }
            }

            return builder.ToString();
        }

        private static void Append(StringBuilder builder, in ReadOnlySequence<byte> readOnlySequence)
        {
            // 异步方法里面不能使用ReadOnlySpan
            ReadOnlySpan<byte> span = readOnlySequence.IsSingleSegment ? readOnlySequence.First.Span : readOnlySequence.ToArray().AsSpan();
            builder.Append(Encoding.UTF8.GetString(span));
        }
    }
}
