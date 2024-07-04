using EES.Infrastructure.DataBase;
using EES.Infrastructure.Entities;
using EES.Infrastructure.Log;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EES.Infrastructure.Service
{
    /// <summary>
    /// 后台日志处理服务
    /// </summary>
    public class LogHandlerService : IHostedService
    {
        private readonly Channel<LogEntityBase> _logChannel;

        private readonly ILogger<LogHandlerService> _logger;

        private readonly IServiceProvider _serviceProvider;

        private Task _logTask;

        public LogHandlerService(Channel<LogEntityBase> logChannel, ILogger<LogHandlerService> logger, IServiceProvider serviceProvider)
        {
            _logChannel = logChannel ?? throw new ArgumentNullException(nameof(logChannel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logTask = Task.Factory.StartNew(async () =>
             {
                 _logger.LogInformation("LogHandlerService Start!");
                 while (await _logChannel.Reader.WaitToReadAsync(cancellationToken))
                 {
                     if (_logChannel.Reader.TryRead(out LogEntityBase logEntity))
                     {
                         await WriteLogAsync(logEntity, cancellationToken);
                     }

                     await Task.Delay(5, cancellationToken);
                 }
             }, TaskCreationOptions.LongRunning);

            return Task.CompletedTask;
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LogHandlerService Stoping !");

            int count = 0;

            while (count < 10)
            {
                if (_logChannel.Writer.TryComplete())
                {
                    await Task.WhenAll(_logTask, _logChannel.Reader.Completion);
                    _logger.LogInformation("LogHandlerService Stoped !");
                    return;
                }

                await Task.Delay(20, cancellationToken);
                count++;
            }

            _logger.LogInformation("Count exceeds 10 times, forced shutdown");

            Environment.Exit(0);
        }

        private async Task WriteLogAsync(LogEntityBase logEntity, CancellationToken cancellationToken)
        {
            if (logEntity is null)
            {
                _logger.LogInformation("WriteLogAsync logEntity is Null!");

                return;
            }

            try
            {
                using var scoped = _serviceProvider.CreateScope();

                var logContext = scoped.ServiceProvider.GetRequiredService<LogDbContext>();

                if (logContext != null)
                {
                    logEntity.CreateTime = DateTime.Now;

                    logContext.Add(logEntity);

                    var res = await logContext.SaveChangesAsync(cancellationToken);

                    if (res <= 0)
                    {
                        _logger.LogInformation("日志入库失败! log:{log}", JsonConvert.SerializeObject(logEntity));
                    }
                }
                else
                {
                    _logger.LogInformation("未能获取到日志数据库的DbContext");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "日志入库发生异常! log:{log}", JsonConvert.SerializeObject(logEntity));

            }
        }


    }
}
